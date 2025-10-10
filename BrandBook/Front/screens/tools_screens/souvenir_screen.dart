import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class SouvenirOption {
  final String name;
  final double price;
  final IconData icon;

  SouvenirOption({required this.name, required this.price, required this.icon});
}

class SouvenirScreen extends StatefulWidget {
  const SouvenirScreen({super.key});

  @override
  State<SouvenirScreen> createState() => _SouvenirScreenState();
}

class _SouvenirScreenState extends State<SouvenirScreen> {
  final List<SouvenirOption> options = [
    SouvenirOption(name: 'Кружки', price: 400, icon: Icons.coffee),
    SouvenirOption(name: 'Футболки', price: 800, icon: Icons.emoji_people),
    SouvenirOption(name: 'Ручки', price: 100, icon: Icons.edit),
    SouvenirOption(name: 'Блокноты', price: 250, icon: Icons.book),
  ];

  SouvenirOption? selectedOption;
  int quantity = 1;
  double? totalCost;

  void calculatePrice() {
    if (selectedOption == null) return; // без выбора — ничего не делаем

    setState(() {
      totalCost = selectedOption!.price * quantity;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Сувенирная продукция')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите продукцию:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),

            ...options.map((option) {
              final isSelected = option == selectedOption;

              return Card(
                color: isSelected ? const Color(0xFFFFD700) : null,
                elevation: isSelected ? 3 : 1,
                shape: RoundedRectangleBorder(
                  side: isSelected
                      ? const BorderSide(color: Colors.black)
                      : BorderSide.none,
                  borderRadius: BorderRadius.circular(10),
                ),
                child: ListTile(
                  leading: Icon(option.icon, size: 36, color: Colors.black),
                  title: Text(
                    option.name,
                    style: const TextStyle(fontWeight: FontWeight.w600),
                  ),
                  subtitle: Text(
                    'Цена за штуку: ${option.price.toStringAsFixed(0)} ₽',
                  ),
                  trailing: isSelected
                      ? const Icon(Icons.check_circle, color: Colors.black)
                      : null,
                  onTap: () => setState(() => selectedOption = option),
                ),
              );
            }).toList(),

            const SizedBox(height: 20),
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
            const SizedBox(height: 20),

            ElevatedButton(
              onPressed: selectedOption == null
                  ? null // кнопка неактивна, пока не выбрали
                  : calculatePrice,
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 14),
              ),
              child: const Text('Рассчитать стоимость'),
            ),

            const SizedBox(height: 24),

            if (totalCost != null && selectedOption != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Сувенирная продукция',
                description: '${selectedOption!.name}, $quantity шт',
              ),

            const SizedBox(height: 32),
            const Text(
              'Примечание:',
              style: TextStyle(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 8),
            const Text(
              'Цена ориентировочная и может зависеть от макета, цветности, количества и типа нанесения.',
              style: TextStyle(color: Colors.black),
            ),
          ],
        ),
      ),
    );
  }
}
