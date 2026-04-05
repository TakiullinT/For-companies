package com.example.config;

import io.tarantool.driver.api.TarantoolClient;
import io.tarantool.driver.api.TarantoolClientFactory;
import io.tarantool.driver.api.tuple.TarantoolTuple;
import io.tarantool.driver.api.TarantoolResult;
import io.tarantool.driver.auth.SimpleTarantoolCredentials;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class TarantoolConfig
{
    @Bean
    public TarantoolClient<TarantoolTuple, TarantoolResult<TarantoolTuple>> tarantoolClient(
            @Value("${spring.data.tarantool.nodes}") String node,
            @Value("${spring.data.tarantool.user-name}") String user,
            @Value("${spring.data.tarantool.password}") String password) {

        String[] hostPort = node.split(":");
        String host = hostPort[0];
        int port = Integer.parseInt(hostPort[1]);

        SimpleTarantoolCredentials credentials = new SimpleTarantoolCredentials(user, password);

        return TarantoolClientFactory.createClient()
                .withAddress(host, port)
                .withCredentials(credentials)
                .build();
    }
}