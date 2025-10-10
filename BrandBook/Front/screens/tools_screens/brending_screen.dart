import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class BrandingOption {
  final String name;
  final String description;
  final double price;
  final IconData icon;

  BrandingOption({
    required this.name,
    required this.description,
    required this.price,
    required this.icon,
  });
}

class BrandingScreen extends StatefulWidget {
  const BrandingScreen({super.key});

  @override
  State<BrandingScreen> createState() => _BrandingScreenState();
}

class _BrandingScreenState extends State<BrandingScreen> {
  final List<BrandingOption> brandingOptions = [
    BrandingOption(
      name: 'Разработка логотипа',
      description: 'Создание уникального логотипа для вашего бренда.',
      price: 10000,
      icon: Icons.brush,
    ),
    BrandingOption(
      name: 'Фирменный стиль',
      description: 'Определение фирменных цветов, шрифтов и элементов.',
      price: 20000,
      icon: Icons.palette,
    ),
    BrandingOption(
      name: 'Брендбук',
      description: 'Создание полноценного гида по использованию бренда.',
      price: 30000,
      icon: Icons.book,
    ),
    BrandingOption(
      name: 'Ребрендинг',
      description: 'Обновление бренда с учетом новой стратегии.',
      price: 25000,
      icon: Icons.refresh,
    ),
  ];

  BrandingOption? selectedOption;
  int quantity = 1;
  double? totalCost;

  void calculateCost() {
    if (selectedOption == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Пожалуйста, выберите тип услуги')),
      );
      return;
    }
    setState(() {
      totalCost = selectedOption!.price * quantity;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Брендинг')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите услугу:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),

            ...brandingOptions.map((option) {
              final isSelected = option == selectedOption;
              return Card(
                color: isSelected ? Color(0xFFFFD700) : null,
                elevation: isSelected ? 4 : 1,
                margin: const EdgeInsets.symmetric(vertical: 6),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8),
                  side: isSelected
                      ? BorderSide(color: Colors.black)
                      : BorderSide.none,
                ),
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
                  onTap: () => setState(() => selectedOption = option),
                ),
              );
            }).toList(),

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

            const SizedBox(height: 24),
            ElevatedButton(
              onPressed: calculateCost,
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
              child: const Text('Рассчитать стоимость'),
            ),

            const SizedBox(height: 24),
            if (totalCost != null && selectedOption != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Брендинг',
                description: '${selectedOption!.name}, $quantity шт',
              ),

            const SizedBox(height: 32),
            const Text(
              'Описание услуги:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            const Text(
              'Брендинг включает в себя разработку визуального образа компании: логотип, фирменные цвета, '
              'шрифты и другие элементы. Это помогает повысить узнаваемость и доверие к бренду.',
            ),
          ],
        ),
      ),
    );
  }
}
