import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class LaserCuttingScreen extends StatefulWidget {
  const LaserCuttingScreen({super.key});

  @override
  State<LaserCuttingScreen> createState() => _LaserCuttingScreenState();
}

class _LaserCuttingScreenState extends State<LaserCuttingScreen> {
  final List<String> materials = ['Акрил', 'Дерево', 'Металл'];
  String selectedMaterial = 'Акрил';
  double width = 1.0;
  double height = 1.0;
  int quantity = 1;
  double? totalCost;

  double calculateCost() {
    double ratePerSqM;

    switch (selectedMaterial) {
      case 'Акрил':
        ratePerSqM = 1500;
        break;
      case 'Дерево':
        ratePerSqM = 1200;
        break;
      case 'Металл':
        ratePerSqM = 2500;
        break;
      default:
        ratePerSqM = 1500;
    }

    double area = width * height;
    return ratePerSqM * area * quantity;
  }

  Widget _buildMaterialSelector() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Материал:',
          style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 12),
        Wrap(
          spacing: 10,
          children: materials.map((material) {
            final bool isSelected = material == selectedMaterial;
            return ChoiceChip(
              label: Text(
                material,
                style: TextStyle(
                  color: isSelected ? Colors.black : Colors.black87,
                  fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                ),
              ),
              selected: isSelected,
              onSelected: (_) => setState(() => selectedMaterial = material),
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

  Widget _buildNumberField({
    required String label,
    required String initialValue,
    required Function(String) onChanged,
  }) {
    return TextFormField(
      initialValue: initialValue,
      keyboardType: TextInputType.number,
      decoration: InputDecoration(
        labelText: label,
        border: const OutlineInputBorder(),
      ),
      onChanged: onChanged,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Лазерная резка')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            _buildMaterialSelector(),
            const SizedBox(height: 24),
            Row(
              children: [
                Expanded(
                  child: _buildNumberField(
                    label: 'Ширина (м)',
                    initialValue: width.toString(),
                    onChanged: (value) =>
                        setState(() => width = double.tryParse(value) ?? 1.0),
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: _buildNumberField(
                    label: 'Высота (м)',
                    initialValue: height.toString(),
                    onChanged: (value) =>
                        setState(() => height = double.tryParse(value) ?? 1.0),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            _buildNumberField(
              label: 'Количество',
              initialValue: quantity.toString(),
              onChanged: (value) =>
                  setState(() => quantity = int.tryParse(value) ?? 1),
            ),
            const SizedBox(height: 24),
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
            const SizedBox(height: 24),
            if (totalCost != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Лазерная резка',
                description:
                    '$selectedMaterial, ${width}x$height м, $quantity шт',
              ),
            const SizedBox(height: 32),
            const Text(
              'Описание услуги:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            const Text(
              'Лазерная резка позволяет точно и быстро вырезать детали из различных материалов. '
              'Подходит для рекламных изделий, табличек, декоративных элементов и многого другого.',
            ),
          ],
        ),
      ),
    );
  }
}
