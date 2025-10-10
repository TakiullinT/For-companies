import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';

class OrderHistoryScreen extends StatefulWidget {
  const OrderHistoryScreen({super.key});

  @override
  State<OrderHistoryScreen> createState() => _OrderHistoryScreenState();
}

class _OrderHistoryScreenState extends State<OrderHistoryScreen> {
  Map<String, List<Map<String, dynamic>>> groupedOrders = {};
  bool isLoading = true;

  @override
  void initState() {
    super.initState();
    loadOrders();
  }

  Future<void> loadOrders() async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('auth_token');

    if (token == null) {
      if (mounted) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(const SnackBar(content: Text('Требуется авторизация')));
      }
      return;
    }

    final response = await http.get(
      Uri.parse('http://127.0.0.1:8000/api/order/'),
      headers: {'Authorization': 'Token $token'},
    );

    if (response.statusCode == 200) {
      final List decoded = jsonDecode(utf8.decode(response.bodyBytes));

      // Сортируем по дате (новые сверху)
      decoded.sort((a, b) {
        final dateA = DateTime.tryParse(a['created_at'] ?? '') ?? DateTime(0);
        final dateB = DateTime.tryParse(b['created_at'] ?? '') ?? DateTime(0);
        return dateB.compareTo(dateA);
      });

      // Группируем
      final Map<String, List<Map<String, dynamic>>> grouped = {};
      final now = DateTime.now();

      for (var order in decoded) {
        final createdAt =
            DateTime.tryParse(order['created_at'] ?? '')?.toLocal() ??
            DateTime(0);

        String groupLabel;
        if (_isSameDay(createdAt, now)) {
          groupLabel = 'Сегодня';
        } else if (_isSameDay(
          createdAt,
          now.subtract(const Duration(days: 1)),
        )) {
          groupLabel = 'Вчера';
        } else {
          groupLabel = DateFormat('d MMMM yyyy', 'ru').format(createdAt);
        }

        grouped.putIfAbsent(groupLabel, () => []).add(order);
      }

      setState(() {
        groupedOrders = grouped;
        isLoading = false;
      });
    } else {
      setState(() => isLoading = false);
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Ошибка загрузки заказов: ${response.statusCode}'),
          ),
        );
      }
    }
  }

  bool _isSameDay(DateTime a, DateTime b) {
    return a.year == b.year && a.month == b.month && a.day == b.day;
  }

  @override
  Widget build(BuildContext context) {
    final groupKeys = groupedOrders.keys.toList();

    return Scaffold(
      appBar: AppBar(title: const Text('История заказов')),
      body: isLoading
          ? const Center(child: CircularProgressIndicator())
          : groupedOrders.isEmpty
          ? const Center(child: Text('История заказов пуста'))
          : ListView.builder(
              itemCount: groupKeys.length,
              itemBuilder: (context, groupIndex) {
                final group = groupKeys[groupIndex];
                final orders = groupedOrders[group]!;

                return Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // Заголовок группы
                    Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Text(
                        group,
                        style: const TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                    // Список заказов в группе
                    ...orders.map((order) {
                      final createdAtStr = order['created_at'] ?? '';
                      DateTime? createdAt = DateTime.tryParse(createdAtStr);
                      if (createdAt != null) {
                        createdAt = createdAt.toLocal();
                      }

                      final formattedTime = createdAt != null
                          ? DateFormat('HH:mm', 'ru').format(createdAt)
                          : '';

                      return Card(
                        margin: const EdgeInsets.symmetric(
                          horizontal: 16,
                          vertical: 4,
                        ),
                        child: Padding(
                          padding: const EdgeInsets.all(16.0),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Услуга: ${order['service'] ?? 'Без названия'}',
                                style: const TextStyle(
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                              const SizedBox(height: 8),
                              Text(
                                'Комментарий: ${order['details'] ?? 'Нет комментария'}',
                              ),
                              const SizedBox(height: 8),
                              Text('Время: $formattedTime'),
                            ],
                          ),
                        ),
                      );
                    }),
                  ],
                );
              },
            ),
    );
  }
}
