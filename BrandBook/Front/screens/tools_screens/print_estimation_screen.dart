import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class PrintOption {
  final String name;
  final String format;
  final double pricePerUnit;
  final IconData icon;
  final String description;

  PrintOption({
    required this.name,
    required this.format,
    required this.pricePerUnit,
    required this.icon,
    required this.description,
  });
}

class PrintEstimationScreen extends StatefulWidget {
  const PrintEstimationScreen({super.key});

  @override
  State<PrintEstimationScreen> createState() => _PrintEstimationScreenState();
}

class _PrintEstimationScreenState extends State<PrintEstimationScreen> {
  final List<PrintOption> printOptions = [
    PrintOption(
      name: 'Листовка',
      format: 'A5',
      pricePerUnit: 3.5,
      icon: Icons.description,
      description: 'Рекламная листовка A5, односторонняя печать.',
    ),
    PrintOption(
      name: 'Буклет',
      format: 'A4',
      pricePerUnit: 6.0,
      icon: Icons.library_books,
      description: 'Сложенный буклет A4 с полноцветной печатью.',
    ),
    PrintOption(
      name: 'Визитка',
      format: 'A6',
      pricePerUnit: 2.0,
      icon: Icons.business,
      description: 'Классическая визитка на плотной бумаге.',
    ),
  ];

  PrintOption? selectedOption;
  int quantity = 100;
  double? totalCost;

  void calculateCost() {
    if (selectedOption == null) return;

    setState(() {
      totalCost = selectedOption!.pricePerUnit * quantity;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Просчёт полиграфии')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите продукцию:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),

            ...printOptions.map((option) {
              final isSelected = selectedOption == option;
              return Card(
                color: isSelected ? const Color(0xFFFFD700) : null,
                elevation: isSelected ? 4 : 1,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8),
                  side: isSelected
                      ? const BorderSide(color: Colors.black)
                      : BorderSide.none,
                ),
                child: ListTile(
                  leading: Icon(option.icon, size: 36, color: Colors.black),
                  title: Text(option.name),
                  subtitle: Text('${option.format} — ${option.description}'),
                  trailing: isSelected
                      ? const Icon(Icons.check_circle, color: Colors.black)
                      : null,
                  onTap: () => setState(() => selectedOption = option),
                ),
              );
            }).toList(),

            const SizedBox(height: 24),
            TextFormField(
              initialValue: quantity.toString(),
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(
                labelText: 'Тираж (шт)',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  quantity = int.tryParse(value) ?? 1;
                });
              },
            ),

            const SizedBox(height: 24),
            ElevatedButton(
              onPressed: selectedOption == null ? null : calculateCost,
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
              child: const Text('Рассчитать стоимость'),
            ),

            const SizedBox(height: 24),
            if (totalCost != null && selectedOption != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Полиграфия',
                description:
                    '${selectedOption!.name} (${selectedOption!.format}), $quantity шт',
              ),
          ],
        ),
      ),
    );
  }
}
