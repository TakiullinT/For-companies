// // import 'package:flutter/material.dart';

// // class CartItem {
// //   final String serviceName;
// //   final String description;
// //   final double price;
// //   int quantity;

// //   CartItem({
// //     required this.serviceName,
// //     required this.description,
// //     required this.price,
// //     this.quantity = 1,
// //   });

// //   double get totalPrice => price * quantity;
// // }

// // class CartProvider extends ChangeNotifier {
// //   final List<CartItem> _items = [];

// //   List<CartItem> get items => _items;

// //   int get itemCount => _items.length;

// //   double get totalPrice {
// //     return _items.fold(0, (sum, item) => sum + item.totalPrice);
// //   }

// //   void addItem(CartItem newItem) {
// //     final index = _items.indexWhere(
// //       (item) => item.serviceName == newItem.serviceName,
// //     );

// //     if (index >= 0) {
// //       _items[index].quantity += newItem.quantity;
// //     } else {
// //       _items.add(newItem);
// //     }

// //     notifyListeners();
// //   }

// //   void removeItem(String serviceName) {
// //     _items.removeWhere((item) => item.serviceName == serviceName);
// //     notifyListeners();
// //   }

// //   void updateQuantity(String serviceName, int newQuantity) {
// //     final index = _items.indexWhere((item) => item.serviceName == serviceName);
// //     if (index >= 0) {
// //       _items[index].quantity = newQuantity;
// //       notifyListeners();
// //     }
// //   }

// //   void clearCart() {
// //     _items.clear();
// //     notifyListeners();
// //   }
// // }
// import 'package:flutter/material.dart';
// import 'package:shared_preferences/shared_preferences.dart';
// import 'dart:convert';

// class CartItem {
//   final String serviceName;
//   final String description;
//   int quantity;
//   double price;

//   CartItem({
//     required this.serviceName,
//     required this.description,
//     this.quantity = 1,
//     required this.price,
//   });

//   double get totalPrice => price * quantity;

//   Map<String, dynamic> toJson() => {
//     'serviceName': serviceName,
//     'description': description,
//     'quantity': quantity,
//     'price': price,
//   };

//   factory CartItem.fromJson(Map<String, dynamic> json) => CartItem(
//     serviceName: json['serviceName'],
//     description: json['description'],
//     quantity: json['quantity'],
//     price: (json['price'] as num).toDouble(),
//   );
// }

// class CartProvider extends ChangeNotifier {
//   String? _authToken;
//   final List<CartItem> _items = [];

//   List<CartItem> get items => List.unmodifiable(_items);
//   int get itemCount => _items.length;
//   double get totalPrice => _items.fold(0, (sum, item) => sum + item.totalPrice);

//   void setAuthToken(String? token) {
//     if (_authToken == token) return;
//     _authToken = token;
//     if (_authToken == null || _authToken!.isEmpty) {
//       _items.clear();
//       notifyListeners();
//     } else {
//       _loadCartFromPrefs();
//     }
//   }

//   Future<void> _loadCartFromPrefs() async {
//     if (_authToken == null) return;
//     final prefs = await SharedPreferences.getInstance();
//     final jsonString = prefs.getString('cart_$_authToken');
//     if (jsonString != null) {
//       final List decoded = json.decode(jsonString);
//       _items
//         ..clear()
//         ..addAll(decoded.map((e) => CartItem.fromJson(e)));
//       notifyListeners();
//     }
//   }

//   Future<void> _saveCartToPrefs() async {
//     if (_authToken == null) return;
//     final prefs = await SharedPreferences.getInstance();
//     final jsonString = json.encode(_items.map((e) => e.toJson()).toList());
//     await prefs.setString('cart_$_authToken', jsonString);
//   }

//   void addItem(CartItem item) {
//     if (_authToken == null) return; // запрет неавторизованным
//     final existingIndex = _items.indexWhere(
//       (e) => e.serviceName == item.serviceName,
//     );
//     if (existingIndex >= 0) {
//       _items[existingIndex].quantity += item.quantity;
//     } else {
//       _items.add(item);
//     }
//     _saveCartToPrefs();
//     notifyListeners();
//   }

//   void updateQuantity(String serviceName, int qty) {
//     final index = _items.indexWhere((e) => e.serviceName == serviceName);
//     if (index >= 0) {
//       _items[index].quantity = qty;
//       _saveCartToPrefs();
//       notifyListeners();
//     }
//   }

//   void removeItem(String serviceName) {
//     _items.removeWhere((e) => e.serviceName == serviceName);
//     _saveCartToPrefs();
//     notifyListeners();
//   }

//   void clearCart() {
//     _items.clear();
//     if (_authToken != null) {
//       SharedPreferences.getInstance().then(
//         (prefs) => prefs.remove('cart_$_authToken'),
//       );
//     }
//     notifyListeners();
//   }
// }
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';

class CartItem {
  final String serviceName;
  final String description;
  int quantity;
  double price;

  CartItem({
    required this.serviceName,
    required this.description,
    this.quantity = 1,
    required this.price,
  });

  double get totalPrice => price * quantity;

  Map<String, dynamic> toJson() => {
    'serviceName': serviceName,
    'description': description,
    'quantity': quantity,
    'price': price,
  };

  factory CartItem.fromJson(Map<String, dynamic> json) => CartItem(
    serviceName: json['serviceName'],
    description: json['description'],
    quantity: json['quantity'],
    price: (json['price'] as num).toDouble(),
  );
}

class CartProvider extends ChangeNotifier {
  String? _authToken;
  final List<CartItem> _items = [];

  List<CartItem> get items => List.unmodifiable(_items);
  int get itemCount => _items.length;
  double get totalPrice =>
      _items.fold(0.0, (sum, item) => sum + item.totalPrice);

  // Вызывается из ProxyProvider при изменении AuthProvider
  void setAuthToken(String? token) {
    if (_authToken == token) return;
    _authToken = token;

    if (_authToken == null || _authToken!.isEmpty) {
      // Если пользователь разлогинился — очищаем корзину и удаляем локальное хранилище
      _items.clear();
      _removeSavedCart();
      notifyListeners();
    } else {
      // Загружаем корзину для нового пользователя (если есть)
      _loadCartFromPrefs();
    }
  }

  Future<void> _loadCartFromPrefs() async {
    if (_authToken == null || _authToken!.isEmpty) return;
    final prefs = await SharedPreferences.getInstance();
    final jsonString = prefs.getString('cart_$_authToken');
    if (jsonString != null) {
      try {
        final List decoded = json.decode(jsonString);
        _items
          ..clear()
          ..addAll(decoded.map((e) => CartItem.fromJson(e)));
      } catch (e) {
        _items.clear();
      }
    } else {
      _items.clear();
    }
    notifyListeners();
  }

  Future<void> _saveCartToPrefs() async {
    if (_authToken == null || _authToken!.isEmpty) return;
    final prefs = await SharedPreferences.getInstance();
    final jsonString = json.encode(_items.map((e) => e.toJson()).toList());
    await prefs.setString('cart_$_authToken', jsonString);
  }

  Future<void> _removeSavedCart() async {
    if (_authToken == null) return;
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('cart_$_authToken');
  }

  void addItem(CartItem item) {
    if (_authToken == null || _authToken!.isEmpty)
      return; // запрет неавторизованным
    final existingIndex = _items.indexWhere(
      (e) => e.serviceName == item.serviceName,
    );
    if (existingIndex >= 0) {
      _items[existingIndex].quantity += item.quantity;
    } else {
      _items.add(item);
    }
    _saveCartToPrefs();
    notifyListeners();
  }

  void updateQuantity(String serviceName, int qty) {
    final index = _items.indexWhere((e) => e.serviceName == serviceName);
    if (index >= 0) {
      if (qty <= 0) {
        _items.removeAt(index);
      } else {
        _items[index].quantity = qty;
      }
      _saveCartToPrefs();
      notifyListeners();
    }
  }

  void removeItem(String serviceName) {
    _items.removeWhere((e) => e.serviceName == serviceName);
    _saveCartToPrefs();
    notifyListeners();
  }

  void clearCart() {
    _items.clear();
    if (_authToken != null && _authToken!.isNotEmpty) {
      SharedPreferences.getInstance().then(
        (prefs) => prefs.remove('cart_$_authToken'),
      );
    }
    notifyListeners();
  }
}
