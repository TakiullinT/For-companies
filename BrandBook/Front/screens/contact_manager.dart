import 'package:flutter/material.dart';

class ContactManagerScreen extends StatefulWidget {
  @override
  _ContactManagerScreenState createState() => _ContactManagerScreenState();
}

class _ContactManagerScreenState extends State<ContactManagerScreen> {
  String? _selectedService;
  String? _assignedManager;

  final Map<String, String> serviceManagers = {
    'Радио': 'Марина',
    'Наружка': 'Екатерина',
    'Полиграфия': 'Александра',
    'Широкоформатная печать': 'Елена',
    'Общая консультация': 'Мария',
  };

  void _saveSelection() {
    if (_selectedService != null) {
      setState(() {
        _assignedManager = serviceManagers[_selectedService];
      });

      showDialog(
        context: context,
        builder: (_) => AlertDialog(
          title: const Text('Сохранено'),
          content: Text(
            'Для "$_selectedService" назначен менеджер: $_assignedManager.',
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('Ок'),
            ),
          ],
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final services = serviceManagers.keys.toList();

    return Scaffold(
      appBar: AppBar(title: const Text('Выбор менеджера')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            ...services.map((service) {
              return RadioListTile<String>(
                title: Text(service),
                value: service,
                groupValue: _selectedService,
                onChanged: (value) {
                  setState(() {
                    _selectedService = value;
                  });
                },
              );
            }).toList(),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: _selectedService == null ? null : _saveSelection,
              child: const Text('Сохранить'),
            ),
            if (_assignedManager != null) ...[
              const SizedBox(height: 30),
              Text(
                'Менеджер для $_selectedService: $_assignedManager',
                style: const TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}
