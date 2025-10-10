import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:http/http.dart' as http;
import '../cart/providers/auth_provider.dart';
import 'package:shared_preferences/shared_preferences.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  bool _isLoading = false;

  Future<void> _login() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _isLoading = true);

    final url = Uri.parse('http://127.0.0.1:8000/api/login/');
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({
        'email': _emailController.text.trim(),
        'password': _passwordController.text.trim(),
      }),
    );

    setState(() => _isLoading = false);

    //   if (response.statusCode == 200) {
    //     final data = jsonDecode(response.body);
    //     final token = data['token'] as String;
    //     final name = data['name'];
    //     final email = data['email'] ?? _emailController.text.trim();

    //     final prefs = await SharedPreferences.getInstance();
    //     await prefs.setString('auth_token', token);
    //     await prefs.setString('user_name', name);
    //     await prefs.setString('user_email', email);

    //     if (!mounted) return;
    //     Navigator.pop(context, true);
    //   } else {
    //     ScaffoldMessenger.of(context).showSnackBar(
    //       const SnackBar(content: Text('Неверный логин или пароль')),
    //     );
    //   }
    // }
    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final token = data['token'] as String;
      final name = data['name'];
      final email = data['email'] ?? _emailController.text.trim();

      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('auth_token', token);
      await prefs.setString('user_name', name);
      await prefs.setString('user_email', email);

      // <-- обновляем состояние AuthProvider
      final authProvider = Provider.of<AuthProvider>(context, listen: false);
      authProvider.login(
        token,
        name,
        email,
      ); // нужно добавить такой метод в AuthProvider

      if (!mounted) return;
      Navigator.pop(context, true);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Вход в личный кабинет')),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24),
          child: Card(
            elevation: 6,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
            child: Padding(
              padding: const EdgeInsets.all(24),
              child: Form(
                key: _formKey,
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Icon(
                      Icons.lock_outline,
                      size: 64,
                      color: Colors.black,
                    ),
                    const SizedBox(height: 16),
                    const Text(
                      'Авторизация',
                      style: TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 24),
                    TextFormField(
                      controller: _emailController,
                      keyboardType: TextInputType.emailAddress,
                      decoration: InputDecoration(
                        prefixIcon: const Icon(Icons.email_outlined),
                        labelText: 'Email',
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(10),
                        ),
                      ),
                      validator: (value) {
                        final regex = RegExp(r'^[^@]+@[^@]+\.[^@]+');
                        if (value == null || value.isEmpty) {
                          return 'Введите email';
                        }
                        if (!regex.hasMatch(value)) {
                          return 'Некорректный email';
                        }
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    TextFormField(
                      controller: _passwordController,
                      obscureText: true,
                      decoration: InputDecoration(
                        prefixIcon: const Icon(Icons.lock),
                        labelText: 'Пароль',
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(10),
                        ),
                      ),
                      validator: (value) =>
                          value!.isEmpty ? 'Введите пароль' : null,
                    ),
                    const SizedBox(height: 24),
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton.icon(
                        icon: const Icon(Icons.login),
                        label: _isLoading
                            ? const CircularProgressIndicator(
                                color: Colors.white,
                                strokeWidth: 2,
                              )
                            : const Text('Войти'),
                        onPressed: _isLoading ? null : _login,
                        style: ElevatedButton.styleFrom(
                          padding: const EdgeInsets.symmetric(vertical: 14),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(10),
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 12),
                    TextButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (_) => const RegisterScreen(),
                          ),
                        );
                      },
                      child: const Text('Нет аккаунта? Зарегистрироваться'),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _nameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  bool _isLoading = false;

  Future<void> _register() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _isLoading = true);

    final url = Uri.parse('http://127.0.0.1:8000/api/register/');
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json; charset=utf-8'},
      body: jsonEncode({
        'name': _nameController.text.trim(),
        'email': _emailController.text.trim(),
        'password': _passwordController.text.trim(),
      }),
    );

    setState(() => _isLoading = false);

    if (response.statusCode == 201) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Регистрация успешна. Теперь войдите.')),
      );
      Navigator.pop(context);
    } else {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Ошибка регистрации')));
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Регистрация')),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24),
          child: Card(
            elevation: 6,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
            child: Padding(
              padding: const EdgeInsets.all(24),
              child: Form(
                key: _formKey,
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    TextFormField(
                      controller: _nameController,
                      decoration: const InputDecoration(
                        labelText: 'Имя',
                        border: OutlineInputBorder(),
                      ),
                      validator: (value) =>
                          value == null || value.isEmpty ? 'Введите имя' : null,
                    ),
                    const SizedBox(height: 16),
                    TextFormField(
                      controller: _emailController,
                      keyboardType: TextInputType.emailAddress,
                      decoration: const InputDecoration(
                        labelText: 'Email',
                        border: OutlineInputBorder(),
                      ),
                      validator: (value) {
                        final regex = RegExp(r'^[^@]+@[^@]+\.[^@]+');
                        if (value == null || value.isEmpty)
                          return 'Введите email';
                        if (!regex.hasMatch(value)) return 'Некорректный email';
                        return null;
                      },
                    ),
                    const SizedBox(height: 16),
                    TextFormField(
                      controller: _passwordController,
                      obscureText: true,
                      decoration: const InputDecoration(
                        labelText: 'Пароль',
                        border: OutlineInputBorder(),
                      ),
                      validator: (value) => value == null || value.isEmpty
                          ? 'Введите пароль'
                          : null,
                    ),
                    const SizedBox(height: 24),
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton(
                        onPressed: _isLoading ? null : _register,
                        child: _isLoading
                            ? const CircularProgressIndicator(
                                color: Colors.white,
                                strokeWidth: 2,
                              )
                            : const Text('Зарегистрироваться'),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
