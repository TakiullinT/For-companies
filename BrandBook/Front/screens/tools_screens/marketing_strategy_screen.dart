import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/chat_with_manager_screen.dart';

class MarketingStrategyScreen extends StatelessWidget {
  const MarketingStrategyScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final List<String> steps = [
      'Анализ целевой аудитории и конкурентов',
      'Определение позиционирования бренда',
      'Выбор маркетинговых каналов',
      'Разработка контент-стратегии',
      'План запуска и KPI',
      'Отслеживание и корректировка стратегии',
    ];

    return Scaffold(
      appBar: AppBar(title: const Text('Маркетинговая стратегия')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Этапы разработки стратегии:',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),
            Expanded(
              child: ListView.separated(
                itemCount: steps.length,
                separatorBuilder: (_, __) => const SizedBox(height: 12),
                itemBuilder: (context, index) {
                  return Card(
                    elevation: 2,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Text(
                        '${index + 1}. ${steps[index]}',
                        style: const TextStyle(fontSize: 16),
                      ),
                    ),
                  );
                },
              ),
            ),
            const SizedBox(height: 16),
            Center(
              child: SizedBox(
                width: double.infinity,
                child: ElevatedButton.icon(
                  icon: const Icon(Icons.chat_bubble_outline),
                  label: const Text(
                    'Связаться с менеджером',
                    style: TextStyle(color: Colors.white),
                  ),
                  style: ElevatedButton.styleFrom(
                    padding: const EdgeInsets.symmetric(vertical: 14),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                    backgroundColor: Colors.black,
                  ),
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (_) =>
                            const ChatWithManagerScreen(managerName: 'Анна'),
                      ),
                    );
                  },
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
