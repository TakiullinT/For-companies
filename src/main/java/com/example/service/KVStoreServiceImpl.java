package com.example.service;

import com.example.grpc.generated.*;
import com.google.protobuf.ByteString;
import io.grpc.Status;
import io.grpc.stub.StreamObserver;
import io.tarantool.driver.api.TarantoolClient;
import io.tarantool.driver.api.TarantoolResult;
import io.tarantool.driver.api.conditions.Conditions;
import io.tarantool.driver.api.tuple.TarantoolTuple;
import io.tarantool.driver.core.tuple.TarantoolTupleImpl;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;

@Service
@RequiredArgsConstructor
public class KVStoreServiceImpl extends KVStoreServiceGrpc.KVStoreServiceImplBase {
    private final TarantoolClient<TarantoolTuple, TarantoolResult<TarantoolTuple>> client;
    private static final String SPACE_NAME = "KV";
    private static final int BATCH_SIZE = 1000;

    @Override
    public void put(PutRequest request, StreamObserver<PutResponse> responseStreamObserver) {
        String key = request.getKey();
        byte[] value = request.hasValue() ? request.getValue().toByteArray() : null;

        List<Object> tupleData = Arrays.asList(key, value);
        TarantoolTuple tuple = new TarantoolTupleImpl(tupleData, client.getConfig().getMessagePackMapper());

        client.space(SPACE_NAME).replace(tuple).thenAccept(result ->
        {
            responseStreamObserver.onNext(PutResponse.newBuilder().setSuccess(true).build());
            responseStreamObserver.onCompleted();
        }).exceptionally(exception ->
        {
            responseStreamObserver.onError(exception);
            return null;
        });
    }

    @Override
    public void get(GetRequest request, StreamObserver<GetResponse> responseStreamObserver) {
        Conditions conditions = Conditions.equals("key", request.getKey());
        client.space(SPACE_NAME).select(conditions).thenAccept(result -> {
            GetResponse.Builder builder = GetResponse.newBuilder();
            if (result != null && !result.isEmpty()) {
                builder.setFound(true);
                TarantoolTuple tuple = result.iterator().next();
                byte[] value = tuple.getByteArray(1);
                if (value != null) {
                    builder.setValue(ByteString.copyFrom(value));
                }
            } else {
                builder.setFound(false);
            }
            responseStreamObserver.onNext(builder.build());
            responseStreamObserver.onCompleted();
        });
    }

    @Override
    public void count(CountRequest request, StreamObserver<CountResponse> responseStreamObserver) {
        client.eval("return box.space.KV:count()")
                .thenAccept(result -> {
                    long count = ((Number) result.getFirst()).longValue();

                    responseStreamObserver.onNext(CountResponse
                            .newBuilder()
                            .setCount(count)
                            .build()
                    );
                    responseStreamObserver.onCompleted();
                })
                .exceptionally(exception -> {
                    responseStreamObserver.onError(exception);
                    return null;
                });
    }

    @Override
    public void delete(DeleteRequest request, StreamObserver<DeleteResponse> responseStreamObserver) {
        Conditions conditions = Conditions.equals("key", request.getKey());
        client.space(SPACE_NAME).delete(conditions)
                .thenAccept(result -> {
                    responseStreamObserver.onNext(DeleteResponse.newBuilder().setSuccess(true).build());
                    responseStreamObserver.onCompleted();
                });
    }

    @Override
    public void range(RangeRequest request, StreamObserver<KeyValueEntry> responseStreamObserver) {
        fetchRangeBatch(request.getKeySince(), request.getKeyTo(), responseStreamObserver);
    }

    private void fetchRangeBatch(String currentKey, String toKey, StreamObserver<KeyValueEntry> observer) {
        Conditions conditions = Conditions.limit(BATCH_SIZE).andGreaterOrEquals("key", currentKey);

        client.space(SPACE_NAME).select(conditions).thenAccept(batch -> {
            if (batch == null || batch.isEmpty()) {
                observer.onCompleted();
                return;
            }

            String lastProcessedKey = null;
            for (TarantoolTuple tuple : batch) {
                String key = tuple.getString(0);

                if (key.compareTo(toKey) > 0) {
                    observer.onCompleted();
                    return;
                }

                KeyValueEntry.Builder entry = KeyValueEntry.newBuilder().setKey(key);
                byte[] value = tuple.getByteArray(1);
                if (value != null) {
                    entry.setValue(ByteString.copyFrom(value));
                }

                observer.onNext(entry.build());
                lastProcessedKey = key;
            }

            if (batch.size() == BATCH_SIZE) {
                fetchRangeBatch(lastProcessedKey + "\0", toKey, observer);
            } else {
                observer.onCompleted();
            }

        }).exceptionally(exception -> {
            observer.onError(Status.INTERNAL.withDescription(exception.getMessage()).asRuntimeException());
            return null;
        });
    }
}