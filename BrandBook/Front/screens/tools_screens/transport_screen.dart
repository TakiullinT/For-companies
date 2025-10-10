import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class TransportBrandingScreen extends StatefulWidget {
  const TransportBrandingScreen({super.key});

  @override
  State<TransportBrandingScreen> createState() =>
      _TransportBrandingScreenState();
}

class _TransportBrandingScreenState extends State<TransportBrandingScreen> {
  final List<Map<String, dynamic>> vehicleTypes = [
    {'label': 'Легковой автомобиль', 'icon': Icons.directions_car},
    {'label': 'Фургон', 'icon': Icons.local_shipping},
    {'label': 'Автобус', 'icon': Icons.directions_bus},
    {'label': 'Грузовик', 'icon': Icons.fire_truck},
  ];

  String? selectedType;
  int quantity = 1;
  double? totalCost;

  double calculateCost() {
    double basePrice;

    switch (selectedType) {
      case 'Легковой автомобиль':
        basePrice = 15000;
        break;
      case 'Фургон':
        basePrice = 25000;
        break;
      case 'Автобус':
        basePrice = 40000;
        break;
      case 'Грузовик':
        basePrice = 35000;
        break;
      default:
        basePrice = 0;
    }

    return basePrice * quantity;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Брендирование транспорта')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите тип транспорта:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.w600),
            ),
            const SizedBox(height: 12),

            GridView.count(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              crossAxisCount: 2,
              crossAxisSpacing: 8,
              mainAxisSpacing: 8,
              childAspectRatio: 1.5,
              children: vehicleTypes.map((type) {
                final isSelected = selectedType == type['label'];
                return GestureDetector(
                  onTap: () {
                    setState(() => selectedType = type['label']);
                  },
                  child: Card(
                    color: isSelected ? const Color(0xFFFFD700) : null,
                    elevation: isSelected ? 2 : 1,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(8),
                      side: isSelected
                          ? const BorderSide(color: Colors.black)
                          : BorderSide.none,
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(type['icon'], size: 40, color: Colors.black),
                        const SizedBox(height: 8),
                        Text(
                          type['label'],
                          textAlign: TextAlign.center,
                          style: const TextStyle(
                            fontSize: 14,
                            fontWeight: FontWeight.bold,
                            color: Colors.black,
                          ),
                        ),
                      ],
                    ),
                  ),
                );
              }).toList(),
            ),

            const SizedBox(height: 24),
            TextFormField(
              initialValue: quantity.toString(),
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(
                labelText: 'Количество транспорта',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  quantity = int.tryParse(value) ?? 1;
                });
              },
            ),
            const SizedBox(height: 24),
            ElevatedButton.icon(
              icon: const Icon(Icons.calculate),
              onPressed: selectedType == null
                  ? null
                  : () {
                      setState(() {
                        totalCost = calculateCost();
                      });
                    },
              label: const Text('Рассчитать стоимость'),
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 16),
                textStyle: const TextStyle(fontSize: 16),
              ),
            ),
            const SizedBox(height: 24),
            AddToCartSection(
              totalCost: totalCost!,
              serviceName: 'Брендирование транспорта',
              description: selectedType != null
                  ? '$selectedType, $quantity шт'
                  : 'Тип транспорта не выбран',
            ),
            const SizedBox(height: 32),
            const Text(
              'Описание услуги:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            const Text(
              'Брендирование транспорта — это эффективный способ продвижения бренда. '
              'Автомобиль с фирменной графикой становится мобильной рекламой, которая работает 24/7. '
              'Выберите нужный тип транспорта и рассчитайте примерную стоимость прямо сейчас.',
              style: TextStyle(fontSize: 14),
            ),
          ],
        ),
      ),
    );
  }
}
