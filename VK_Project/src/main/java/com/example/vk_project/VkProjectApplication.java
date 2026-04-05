package com.example.vk_project;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication(scanBasePackages = "com.example")
public class VkProjectApplication {

    public static void main(String[] args) {
        SpringApplication.run(VkProjectApplication.class, args);
    }
}
