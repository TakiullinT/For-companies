import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class ApiService {
  final String baseUrl = 'http://127.0.0.1:8000';

  Future<bool> addOrder(String service, String details, String s) async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('auth_token');

    if (token == null || token.isEmpty) {
      throw Exception('Не авторизован. Токен не найден.');
    }

    final response = await http.post(
      Uri.parse('$baseUrl/api/order/'),
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Token $token',
      },
      body: jsonEncode({'service': service, 'details': details}),
    );

    if (response.statusCode == 200) {
      final jsonResponse = jsonDecode(response.body);
      return jsonResponse['message'] == 'Order added';
    } else {
      print('Ошибка: ${utf8.decode(response.bodyBytes)}');
      return false;
    }
  }
}
