import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class SignboardOption {
  final String name;
  final String description;
  final double basePricePerSqM;
  final IconData icon;

  SignboardOption({
    required this.name,
    required this.description,
    required this.basePricePerSqM,
    required this.icon,
  });
}

class SignboardsScreen extends StatefulWidget {
  const SignboardsScreen({super.key});

  @override
  State<SignboardsScreen> createState() => _SignboardsScreenState();
}

class _SignboardsScreenState extends State<SignboardsScreen> {
  final List<SignboardOption> signboardOptions = [
    SignboardOption(
      name: 'Световая вывеска',
      description: 'Яркая вывеска с подсветкой для привлечения внимания.',
      basePricePerSqM: 7000,
      icon: Icons.lightbulb_outline,
    ),
    SignboardOption(
      name: 'Несветовая вывеска',
      description: 'Вывеска без подсветки, но с эффектным дизайном.',
      basePricePerSqM: 5000,
      icon: Icons.signpost_outlined,
    ),
    SignboardOption(
      name: 'Объемные буквы',
      description: 'Объемные буквы с глубиной для эффектного вида.',
      basePricePerSqM: 8000,
      icon: Icons.font_download,
    ),
    SignboardOption(
      name: 'Вывеска из композита',
      description: 'Прочная и стильная вывеска из композитных материалов.',
      basePricePerSqM: 6500,
      icon: Icons.widgets_outlined,
    ),
  ];

  SignboardOption? selectedOption;
  double width = 2.0;
  double height = 1.0;
  int quantity = 1;
  double? totalCost;

  double calculateCost() {
    if (selectedOption == null) return 0.0;
    double area = width * height;
    return area * selectedOption!.basePricePerSqM * quantity;
  }

  bool isInputValid() {
    return width > 0 && height > 0 && quantity > 0 && selectedOption != null;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Вывески')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите тип вывески:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            ...signboardOptions.map((option) {
              bool isSelected = option == selectedOption;
              return Card(
                margin: const EdgeInsets.symmetric(vertical: 6),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8),
                  side: isSelected
                      ? BorderSide(color: Colors.black, width: 1)
                      : BorderSide.none,
                ),
                color: isSelected ? Color(0xFFFFD700) : null,
                child: ListTile(
                  leading: Icon(option.icon, size: 36, color: Colors.black),
                  title: Text(
                    option.name,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Text(option.description),
                  trailing: isSelected
                      ? const Icon(Icons.check_circle, color: Colors.black)
                      : null,
                  onTap: () => setState(() {
                    selectedOption = option;
                    totalCost = null;
                  }),
                ),
              );
            }).toList(),

            const SizedBox(height: 24),
            TextFormField(
              initialValue: width.toString(),
              keyboardType: TextInputType.numberWithOptions(decimal: true),
              decoration: const InputDecoration(
                labelText: 'Ширина (м)',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  width = double.tryParse(value) ?? 1.0;
                  totalCost = null;
                });
              },
            ),
            const SizedBox(height: 12),
            TextFormField(
              initialValue: height.toString(),
              keyboardType: TextInputType.numberWithOptions(decimal: true),
              decoration: const InputDecoration(
                labelText: 'Высота (м)',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  height = double.tryParse(value) ?? 1.0;
                  totalCost = null;
                });
              },
            ),
            const SizedBox(height: 12),
            TextFormField(
              initialValue: quantity.toString(),
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(
                labelText: 'Количество',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  quantity = int.tryParse(value) ?? 1;
                  totalCost = null;
                });
              },
            ),

            const SizedBox(height: 24),
            ElevatedButton(
              onPressed: isInputValid()
                  ? () {
                      setState(() {
                        totalCost = calculateCost();
                      });
                    }
                  : null,
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
              child: const Text('Рассчитать стоимость'),
            ),

            const SizedBox(height: 24),
            if (totalCost != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Вывески',
                description:
                    '${selectedOption!.name}, ${width.toStringAsFixed(1)}x${height.toStringAsFixed(1)} м, $quantity шт',
              ),

            const SizedBox(height: 32),
            const Text(
              'Описание услуги:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            const Text(
              'Изготавливаем световые и несветовые вывески, объемные буквы, вывески из композита. '
              'Подходят для наружной и внутренней рекламы.',
            ),
          ],
        ),
      ),
    );
  }
}
