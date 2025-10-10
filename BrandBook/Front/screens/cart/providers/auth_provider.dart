// import 'package:flutter/material.dart';
// import 'package:shared_preferences/shared_preferences.dart';
// import '../cart_provider.dart';
// import 'package:provider/provider.dart';

// class AuthProvider extends ChangeNotifier {
//   String? _token;
//   String? _name;
//   String? _email;

//   bool get isAuthenticated => _token != null && _token!.isNotEmpty;
//   String? get token => _token;

//   void login(String token, String name, String email) {
//     _token = token;
//     _name = name;
//     _email = email;
//     notifyListeners();
//   }

//   void logout(BuildContext context) {
//     _token = null;
//     _name = null;
//     _email = null;
//     notifyListeners();
//     //CartProvider().clearCart();
//     final cart = Provider.of<CartProvider>(context, listen: false);
//     cart.clearCart();
//   }
// }

import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

class AuthProvider extends ChangeNotifier {
  String? _token;
  String? _name;
  String? _email;

  bool get isAuthenticated => _token != null && _token!.isNotEmpty;
  String? get token => _token;
  String? get name => _name;
  String? get email => _email;

  Future<void> loadFromPrefs() async {
    final prefs = await SharedPreferences.getInstance();
    _token = prefs.getString('auth_token');
    _name = prefs.getString('auth_name');
    _email = prefs.getString('auth_email');
    notifyListeners();
  }

  Future<void> login(String token, String name, String email) async {
    _token = token;
    _name = name;
    _email = email;
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('auth_token', token);
    await prefs.setString('auth_name', name);
    await prefs.setString('auth_email', email);
    notifyListeners();
  }

  Future<void> logout() async {
    _token = null;
    _name = null;
    _email = null;
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('auth_token');
    await prefs.remove('auth_name');
    await prefs.remove('auth_email');
    await prefs.remove('user_name'); // <--- очищаем профиль
    await prefs.remove('user_email'); // <--- очищаем профиль
    await prefs.remove('user_avatar');
    notifyListeners();
  }
}
