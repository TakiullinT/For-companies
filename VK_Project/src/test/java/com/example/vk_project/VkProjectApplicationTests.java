package com.example.vk_project;

import com.example.grpc.generated.*;
import com.google.protobuf.ByteString;
import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.TestPropertySource;

import java.util.Iterator;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.DEFINED_PORT)
@TestPropertySource(properties = "spring.grpc.server.port=9090")
class VkProjectApplicationTests {

    private static final Logger log = LoggerFactory.getLogger(VkProjectApplicationTests.class);
    private static KVStoreServiceGrpc.KVStoreServiceBlockingStub stub;

    @BeforeAll
    static void setUp() {
        ManagedChannel channel = ManagedChannelBuilder.forAddress("localhost", 9090)
                .usePlaintext()
                .build();
        stub = KVStoreServiceGrpc.newBlockingStub(channel);
    }

    @Test
    @DisplayName("Тест стабильности при 5_000_000 записей")
    void testMassiveDataLoad() {
        int totalCount = 5_000_000;
        int logStep = 100_000;

        log.info("Insert start: {}", totalCount);
        long start = System.currentTimeMillis();

        for (int i = 0; i < totalCount; i++) {
            String key = "key_" + i;
            String value = "value_" + i;

            PutRequest request = PutRequest.newBuilder()
                    .setKey(key)
                    .setValue(ByteString.copyFromUtf8(value))
                    .build();

            PutResponse response = stub.put(request);

            if ((i + 1) % logStep == 0) {
                log.info("Inserted: {}", (i + 1));
            }
        }

        long duration = (System.currentTimeMillis() - start) / 1000;
        log.info("Insert done in {}s", duration);

        log.info("Count check");
        CountResponse countRes = stub.count(CountRequest.newBuilder().build());
        log.info("Total: {}", countRes.getCount());
        assertTrue(countRes.getCount() >= totalCount, "Количество записей меньше ожидаемого");

        log.info("Get check");

        GetResponse mid = stub.get(GetRequest.newBuilder().setKey("key_2500000").build());
        assertTrue(mid.getFound());
        assertEquals("value_2500000", mid.getValue().toStringUtf8());

        GetResponse end = stub.get(GetRequest.newBuilder().setKey("key_4999999").build());
        assertTrue(end.getFound());
        assertEquals("value_4999999", end.getValue().toStringUtf8());
    }

    @Test
    void testPutEmptyValue() {
        String key = "null_test_key";

        PutRequest putRequest = PutRequest.newBuilder()
                .setKey(key)
                .setValue(ByteString.EMPTY)
                .build();
        PutResponse putResponse = stub.put(putRequest);

        GetRequest getRequest = GetRequest.newBuilder().setKey(key).build();
        GetResponse response = stub.get(getRequest);

        assertTrue(response.getFound(), "Record should be found");
        assertTrue(response.getValue().isEmpty(), "Value should be empty ByteString");
    }


    @Test
    void testRangeLargeDataset() {
        RangeRequest request = RangeRequest.newBuilder()
                .setKeySince("key_0")
                .setKeyTo("key_z")
                .build();

        Iterator<KeyValueEntry> responseIterator = stub.range(request);

        int count = 0;
        while (responseIterator.hasNext() && count < 100000) {
            KeyValueEntry entry = responseIterator.next();
            assertNotNull(entry.getKey());
            count++;

            if (count % 10000 == 0) {
                log.info("Received {} records...", count);
            }
        }

        assertEquals(100000, count, "Should receive exactly 100,000 records");
    }
}