import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';

class NewsScreen extends StatefulWidget {
  const NewsScreen({super.key});

  @override
  State<NewsScreen> createState() => _NewsScreenState();
}

class _NewsScreenState extends State<NewsScreen> {
  List<Map<String, dynamic>> newsItems = [];
  bool isLoading = true;
  String errorMessage = '';

  @override
  void initState() {
    super.initState();
    _loadNewsFromApi();
  }

  Future<void> _loadNewsFromApi() async {
    if (!mounted) return;
    setState(() {
      isLoading = true;
      errorMessage = '';
    });

    try {
      final response = await http.get(
        Uri.parse('http://127.0.0.1:8000/api/news/'),
      );

      if (!mounted) return;

      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(utf8.decode(response.bodyBytes));
        setState(() {
          newsItems = data.map((item) {
            final dt = DateTime.tryParse(item['published_at'] ?? '')?.toLocal();
            final formattedDate = dt != null
                ? DateFormat('dd MMM yyyy, HH:mm', 'ru').format(dt)
                : '';

            return {
              'title': item['title'] ?? 'Без названия',
              'desc': item['description'] ?? '',
              'date': formattedDate,
            };
          }).toList();
          isLoading = false;
        });
      } else {
        setState(() {
          errorMessage = 'Ошибка загрузки новостей: ${response.statusCode}';
          isLoading = false;
        });
      }
    } catch (e) {
      if (!mounted) return;
      setState(() {
        errorMessage = 'Не удалось загрузить новости. Проверьте соединение.';
        isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Новости и скидки'),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh),
            onPressed: _loadNewsFromApi,
          ),
        ],
      ),
      body: isLoading
          ? const Center(child: CircularProgressIndicator())
          : errorMessage.isNotEmpty
          ? Center(child: Text(errorMessage))
          : ListView.builder(
              itemCount: newsItems.length,
              itemBuilder: (context, index) {
                final news = newsItems[index];
                return Card(
                  margin: const EdgeInsets.all(10),
                  child: ListTile(
                    leading: const Icon(Icons.discount, color: Colors.red),
                    title: Text(news['title']!),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(news['desc']!),
                        if ((news['date'] as String).isNotEmpty)
                          Padding(
                            padding: const EdgeInsets.only(top: 4),
                            child: Text(
                              news['date']!,
                              style: const TextStyle(
                                fontSize: 12,
                                color: Colors.grey,
                              ),
                            ),
                          ),
                      ],
                    ),
                  ),
                );
              },
            ),
    );
  }
}
