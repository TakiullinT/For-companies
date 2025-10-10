import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class InteriorPrintScreen extends StatefulWidget {
  const InteriorPrintScreen({super.key});

  @override
  State<InteriorPrintScreen> createState() => _InteriorPrintScreenState();
}

class _InteriorPrintScreenState extends State<InteriorPrintScreen> {
  final List<String> materials = ['Баннер', 'Пленка', 'Пластик'];
  final List<String> sizes = ['1x1 м', '2x2 м', '3x3 м'];

  String selectedMaterial = 'Баннер';
  String selectedSize = '2x2 м';
  int quantity = 1;
  double? totalCost;

  double calculateCost() {
    double basePrice;

    switch (selectedMaterial) {
      case 'Баннер':
        basePrice = 1500;
        break;
      case 'Пленка':
        basePrice = 1200;
        break;
      case 'Пластик':
        basePrice = 2000;
        break;
      default:
        basePrice = 1000;
    }

    double sizeMultiplier;

    switch (selectedSize) {
      case '1x1 м':
        sizeMultiplier = 1.0;
        break;
      case '2x2 м':
        sizeMultiplier = 1.8;
        break;
      case '3x3 м':
        sizeMultiplier = 2.5;
        break;
      default:
        sizeMultiplier = 1.0;
    }

    return basePrice * sizeMultiplier * quantity;
  }

  // Widget _buildCardSelector({
  //   required String title,
  //   required List<String> options,
  //   required String selected,
  //   required void Function(String) onSelect,
  // }) {
  //   return Column(
  //     crossAxisAlignment: CrossAxisAlignment.start,
  //     children: [
  //       Text(
  //         title,
  //         style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
  //       ),
  //       const SizedBox(height: 12),
  //       Wrap(
  //         spacing: 10,
  //         children: options.map((option) {
  //           final bool isSelected = option == selected;
  //           return ChoiceChip(
  //             label: Text(option),
  //             selected: isSelected,
  //             onSelected: (_) => onSelect(option),
  //             selectedColor: Colors.blue.shade100,
  //             labelStyle: TextStyle(
  //               color: isSelected ? Colors.blue.shade800 : Colors.black87,
  //             ),
  //             padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
  //           );
  //         }).toList(),
  //       ),
  //     ],
  //   );
  // }
  Widget _buildCardSelector({
    required String title,
    required List<String> options,
    required String selected,
    required void Function(String) onSelect,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          title,
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 12),
        Wrap(
          spacing: 10,
          children: options.map((option) {
            final bool isSelected = option == selected;
            return ChoiceChip(
              label: Text(
                option,
                style: TextStyle(
                  color: isSelected ? Colors.black : Colors.black87,
                  fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                ),
              ),
              selected: isSelected,
              onSelected: (_) => onSelect(option),
              selectedColor: const Color(0xFFFFD700),
              backgroundColor: const Color(0xFFF5F5F5),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(8),
                side: BorderSide(
                  color: isSelected
                      ? const Color(0xFFFFD700)
                      : Colors.grey.shade400,
                ),
              ),
              padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
            );
          }).toList(),
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Интерьерная печать')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            _buildCardSelector(
              title: 'Материал:',
              options: materials,
              selected: selectedMaterial,
              onSelect: (value) => setState(() => selectedMaterial = value),
            ),
            const SizedBox(height: 24),
            _buildCardSelector(
              title: 'Размер:',
              options: sizes,
              selected: selectedSize,
              onSelect: (value) => setState(() => selectedSize = value),
            ),
            const SizedBox(height: 24),
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
                });
              },
            ),
            const SizedBox(height: 28),
            ElevatedButton(
              onPressed: () {
                setState(() {
                  totalCost = calculateCost();
                });
              },
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
              child: const Text('Рассчитать стоимость'),
            ),
            const SizedBox(height: 28),
            if (totalCost != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Интерьерная печать',
                description: '$selectedMaterial, $selectedSize, $quantity шт',
              ),
            const SizedBox(height: 32),
            const Text(
              'О сервисе:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            const Text(
              'Интерьерная печать используется для оформления помещений, стендов и витрин. Выберите нужный материал и размер, и мы произведем качественную печать с учётом ваших задач.',
            ),
          ],
        ),
      ),
    );
  }
}
