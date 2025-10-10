import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class AuthService {
  final String baseUrl = 'http://localhost:3000';

  Future<bool> login(String email, String password) async {
    final url = Uri.parse('$baseUrl/api/login');
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'email': email, 'password': password}),
    );

    if (response.statusCode == 200) {
      final token = jsonDecode(response.body)['token'];
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', token);
      return true;
    }

    return false;
  }

  Future<void> logout() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('token');
  }

  Future<String?> getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token');
  }

  Future<bool> register(String email, String password, String name) async {
    final url = Uri.parse('$baseUrl/api/register');
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'email': email, 'password': password, 'name': name}),
    );

    return response.statusCode == 200;
  }
}
