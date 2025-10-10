import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class OutdoorFormat {
  final String name;
  final String description;
  final double pricePerM2;
  final IconData icon;

  OutdoorFormat({
    required this.name,
    required this.description,
    required this.pricePerM2,
    required this.icon,
  });
}

class OutdoorCalculatorScreen extends StatefulWidget {
  const OutdoorCalculatorScreen({super.key});

  @override
  State<OutdoorCalculatorScreen> createState() =>
      _OutdoorCalculatorScreenState();
}

class _OutdoorCalculatorScreenState extends State<OutdoorCalculatorScreen> {
  final List<OutdoorFormat> formats = [
    OutdoorFormat(
      name: 'Баннер',
      description: 'Легкий и универсальный рекламный материал.',
      pricePerM2: 500,
      icon: Icons.texture,
    ),
    OutdoorFormat(
      name: 'Билборд',
      description: 'Большая поверхность для рекламы на улице.',
      pricePerM2: 700,
      icon: Icons.landscape,
    ),
    OutdoorFormat(
      name: 'Щит',
      description: 'Прочная конструкция для наружной рекламы.',
      pricePerM2: 600,
      icon: Icons.dashboard,
    ),
    OutdoorFormat(
      name: 'Лайтбокс',
      description: 'Подсвечиваемая рекламная конструкция.',
      pricePerM2: 900,
      icon: Icons.light,
    ),
  ];

  OutdoorFormat? selectedFormat;
  double width = 3.0;
  double height = 2.0;
  int quantity = 1;
  double? totalCost;

  void calculateCost() {
    if (selectedFormat == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Пожалуйста, выберите тип продукции')),
      );
      return;
    }

    final area = width * height;
    final cost = area * selectedFormat!.pricePerM2 * quantity;

    setState(() {
      totalCost = cost;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Расчёт наружной рекламы')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите формат конструкции:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),

            ...formats.map((format) {
              final isSelected = format == selectedFormat;
              return Card(
                margin: const EdgeInsets.symmetric(vertical: 6),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8),
                  side: isSelected
                      ? const BorderSide(color: Colors.black, width: 1)
                      : BorderSide.none,
                ),
                color: isSelected ? const Color(0xFFFFD700) : null,
                elevation: isSelected ? 4 : 1,
                child: ListTile(
                  leading: Icon(format.icon, size: 36, color: Colors.black),
                  title: Text(
                    format.name,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Text(format.description),
                  trailing: isSelected
                      ? const Icon(Icons.check_circle, color: Colors.black)
                      : null,
                  onTap: () {
                    setState(() {
                      selectedFormat = format;
                    });
                  },
                ),
              );
            }).toList(),

            const SizedBox(height: 24),
            Row(
              children: [
                Expanded(
                  child: TextFormField(
                    initialValue: width.toString(),
                    keyboardType: const TextInputType.numberWithOptions(
                      decimal: true,
                    ),
                    decoration: const InputDecoration(
                      labelText: 'Ширина (м)',
                      border: OutlineInputBorder(),
                    ),
                    onChanged: (value) {
                      setState(() {
                        width = double.tryParse(value) ?? width;
                      });
                    },
                  ),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: TextFormField(
                    initialValue: height.toString(),
                    keyboardType: const TextInputType.numberWithOptions(
                      decimal: true,
                    ),
                    decoration: const InputDecoration(
                      labelText: 'Высота (м)',
                      border: OutlineInputBorder(),
                    ),
                    onChanged: (value) {
                      setState(() {
                        height = double.tryParse(value) ?? height;
                      });
                    },
                  ),
                ),
              ],
            ),

            const SizedBox(height: 16),
            TextFormField(
              initialValue: quantity.toString(),
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(
                labelText: 'Количество',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  quantity = int.tryParse(value) ?? quantity;
                });
              },
            ),

            const SizedBox(height: 24),
            ElevatedButton(
              onPressed: selectedFormat == null
                  ? null
                  : () {
                      calculateCost();
                    },
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
              child: const Text('Рассчитать стоимость'),
            ),

            const SizedBox(height: 24),
            if (totalCost != null && selectedFormat != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Наружная реклама',
                description:
                    '${selectedFormat!.name} (${width}x$height м), $quantity шт',
              ),
          ],
        ),
      ),
    );
  }
}
