import 'package:flutter/material.dart';

class BriefScreen extends StatefulWidget {
  const BriefScreen({super.key});

  @override
  State<BriefScreen> createState() => _BriefScreenState();
}

class _BriefScreenState extends State<BriefScreen> {
  final _formKey = GlobalKey<FormState>();
  final TextEditingController _companyController = TextEditingController();
  final TextEditingController _goalController = TextEditingController();
  final TextEditingController _featuresController = TextEditingController();
  final TextEditingController _budgetController = TextEditingController();

  final List<Map<String, dynamic>> _projectTypes = [
    {'label': 'Сайт', 'icon': Icons.language},
    {'label': 'Мобильное приложение', 'icon': Icons.phone_android},
  ];

  String? _selectedType;

  void _submitBrief() {
    if (_selectedType == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Пожалуйста, выберите тип проекта')),
      );
      return;
    }

    if (_formKey.currentState!.validate()) {
      showDialog(
        context: context,
        builder: (_) => AlertDialog(
          title: const Text('Бриф отправлен'),
          content: const Text('Мы свяжемся с вами для обсуждения деталей.'),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('ОК'),
            ),
          ],
        ),
      );
    }
  }

  @override
  void dispose() {
    _companyController.dispose();
    _goalController.dispose();
    _featuresController.dispose();
    _budgetController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Бриф на проект')),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              const Text(
                'Тип проекта:',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
              const SizedBox(height: 12),
              Row(
                children: _projectTypes.map((type) {
                  final isSelected = _selectedType == type['label'];
                  return Expanded(
                    child: GestureDetector(
                      onTap: () {
                        setState(() => _selectedType = type['label']);
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
                        child: SizedBox(
                          height: 100,
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Icon(type['icon'], color: Colors.black, size: 40),
                              const SizedBox(height: 8),
                              Text(
                                type['label'],
                                style: TextStyle(
                                  fontWeight: FontWeight.bold,
                                  color: isSelected
                                      ? Colors.black
                                      : Colors.black87,
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
                controller: _companyController,
                decoration: const InputDecoration(
                  labelText: 'Название компании',
                  border: OutlineInputBorder(),
                ),
                validator: (value) =>
                    value!.isEmpty ? 'Введите название компании' : null,
              ),
              const SizedBox(height: 16),

              TextFormField(
                controller: _goalController,
                decoration: const InputDecoration(
                  labelText: 'Цель проекта',
                  border: OutlineInputBorder(),
                ),
                validator: (value) =>
                    value!.isEmpty ? 'Опишите цель проекта' : null,
              ),
              const SizedBox(height: 16),

              TextFormField(
                controller: _featuresController,
                decoration: const InputDecoration(
                  labelText: 'Ключевые функции',
                  border: OutlineInputBorder(),
                ),
                validator: (value) =>
                    value!.isEmpty ? 'Опишите ключевые функции' : null,
              ),
              const SizedBox(height: 16),

              TextFormField(
                controller: _budgetController,
                decoration: const InputDecoration(
                  labelText: 'Ожидаемый бюджет (₽)',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.number,
              ),
              const SizedBox(height: 28),

              ElevatedButton(
                onPressed: _selectedType == null ? null : _submitBrief,
                style: ElevatedButton.styleFrom(
                  padding: const EdgeInsets.symmetric(vertical: 14),
                ),
                child: const Text('Отправить бриф'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
