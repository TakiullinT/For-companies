import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';
import 'package:webview_flutter/webview_flutter.dart';

class SignsMapScreen extends StatefulWidget {
  const SignsMapScreen({super.key});

  @override
  State<SignsMapScreen> createState() => _SignsMapScreenState();
}

class _SignsMapScreenState extends State<SignsMapScreen> {
  late final WebViewController _webViewController;

  final List<Map<String, dynamic>> signTypes = [
    {'label': 'Напольный указатель', 'icon': Icons.stairs},
    {'label': 'Настенный указатель', 'icon': Icons.wallpaper},
    {'label': 'Указатель с подсветкой', 'icon': Icons.light},
  ];

  String? selectedSign;
  int quantity = 1;
  double? totalCost;

  @override
  void initState() {
    super.initState();
    _webViewController = WebViewController()
      ..setJavaScriptMode(JavaScriptMode.unrestricted)
      ..loadRequest(
        Uri.parse(
          'https://www.google.com/maps/d/u/0/embed?mid=1zxUuAhIyYnh4K-N0z7sVMqBbjFw',
        ),
      );
  }

  double calculateCost() {
    double basePrice;
    switch (selectedSign) {
      case 'Напольный указатель':
        basePrice = 3000;
        break;
      case 'Настенный указатель':
        basePrice = 2500;
        break;
      case 'Указатель с подсветкой':
        basePrice = 5000;
        break;
      default:
        basePrice = 0;
    }
    return basePrice * quantity;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Указатели и карта')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите тип указателя:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            Row(
              children: signTypes.map((type) {
                final isSelected = selectedSign == type['label'];
                return Expanded(
                  child: GestureDetector(
                    onTap: () {
                      setState(() => selectedSign = type['label']);
                    },
                    child: Card(
                      color: isSelected ? const Color(0xFFFFD700) : null,
                      elevation: isSelected ? 4 : 1,
                      margin: const EdgeInsets.symmetric(horizontal: 4),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(8),
                        side: isSelected
                            ? const BorderSide(color: Colors.black)
                            : BorderSide.none,
                      ),
                      child: Padding(
                        padding: const EdgeInsets.symmetric(
                          vertical: 12,
                          horizontal: 8,
                        ),
                        child: Column(
                          children: [
                            Icon(type['icon'], size: 30, color: Colors.black),
                            const SizedBox(height: 4),
                            Text(
                              type['label'],
                              textAlign: TextAlign.center,
                              style: TextStyle(
                                fontWeight: FontWeight.bold,
                                color: isSelected
                                    ? Colors.black
                                    : Colors.black87,
                                fontSize: 12,
                              ),
                            ),
                          ],
                        ),
                      ),
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
              onPressed: selectedSign == null
                  ? null
                  : () {
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
            AddToCartSection(
              totalCost: totalCost!,
              serviceName: 'Указатели и карта',
              description: selectedSign != null
                  ? '$selectedSign, $quantity шт'
                  : 'Тип указателя не выбран',
            ),
            const SizedBox(height: 32),
            const Text(
              'Пример размещения указателей на карте:',
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            ClipRRect(
              borderRadius: BorderRadius.circular(12),
              child: SizedBox(
                height: 300,
                child: WebViewWidget(controller: _webViewController),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
