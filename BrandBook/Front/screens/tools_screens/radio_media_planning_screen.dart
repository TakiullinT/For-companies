import 'package:flutter/material.dart';
import 'package:brandbook_app/screens/cart/add_to_cart_section.dart';

class RadioMediaPlanningScreen extends StatefulWidget {
  const RadioMediaPlanningScreen({super.key});

  @override
  State<RadioMediaPlanningScreen> createState() =>
      _RadioMediaPlanningScreenState();
}

class RadioStation {
  final String name;
  final String description;
  final String logoAsset;

  RadioStation({
    required this.name,
    required this.description,
    required this.logoAsset,
  });
}

class _RadioMediaPlanningScreenState extends State<RadioMediaPlanningScreen> {
  final List<RadioStation> stations = [
    RadioStation(
      name: 'Новое Радио',
      description: 'Популярная радиостанция с современными хитами и новостями.',
      logoAsset: 'assets/logos/novoe_radio.png',
    ),
    RadioStation(
      name: 'Студио 21',
      description: 'Музыкальное радио с молодежным контентом и шоу.',
      logoAsset: 'assets/logos/studio21.png',
    ),
    RadioStation(
      name: 'Радио 7',
      description: 'Радио с классикой, джазом и атмосферной музыкой.',
      logoAsset: 'assets/logos/radio7.png',
    ),
  ];

  RadioStation? selectedStation;
  int numberOfSpots = 10;
  int spotDuration = 30;
  double? totalCost;

  void calculateCost() {
    if (selectedStation == null) return;

    double baseRate;
    switch (selectedStation!.name) {
      case 'Новое Радио':
        baseRate = 1200;
        break;
      case 'Студио 21':
        baseRate = 1000;
        break;
      case 'Радио 7':
        baseRate = 1100;
        break;
      default:
        baseRate = 0;
    }

    final result = numberOfSpots * (spotDuration / 30.0) * baseRate;
    setState(() {
      totalCost = result;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Медиапланирование (Радио)')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            const Text(
              'Выберите радиостанцию:',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),

            ...stations.map((station) {
              final isSelected =
                  selectedStation != null &&
                  station.name == selectedStation!.name;
              return Card(
                color: isSelected ? const Color(0xFFFFD700) : null,
                elevation: isSelected ? 4 : 1,
                margin: const EdgeInsets.symmetric(vertical: 6),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(8),
                  side: isSelected
                      ? const BorderSide(color: Colors.black)
                      : BorderSide.none,
                ),
                child: ListTile(
                  leading: Image.asset(
                    station.logoAsset,
                    width: 50,
                    height: 50,
                    fit: BoxFit.contain,
                  ),
                  title: Text(
                    station.name,
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Text(station.description),
                  trailing: isSelected
                      ? const Icon(Icons.check_circle, color: Colors.black)
                      : null,
                  onTap: () {
                    setState(() {
                      selectedStation = station;
                    });
                  },
                ),
              );
            }),

            const SizedBox(height: 24),

            TextFormField(
              initialValue: numberOfSpots.toString(),
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(
                labelText: 'Количество выходов в эфир',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  numberOfSpots = int.tryParse(value) ?? 1;
                });
              },
            ),

            const SizedBox(height: 16),

            TextFormField(
              initialValue: spotDuration.toString(),
              keyboardType: TextInputType.number,
              decoration: const InputDecoration(
                labelText: 'Длительность ролика (секунды)',
                border: OutlineInputBorder(),
              ),
              onChanged: (value) {
                setState(() {
                  spotDuration = int.tryParse(value) ?? 30;
                });
              },
            ),

            const SizedBox(height: 24),

            ElevatedButton(
              onPressed: selectedStation == null
                  ? null
                  : () {
                      calculateCost();
                    },
              child: const Text('Рассчитать стоимость'),
            ),

            const SizedBox(height: 24),

            if (totalCost != null && selectedStation != null)
              AddToCartSection(
                totalCost: totalCost!,
                serviceName: 'Радио и реклама',
                description:
                    '${selectedStation!.name}, $numberOfSpots выходов по $spotDuration сек',
              ),
          ],
        ),
      ),
    );
  }
}
