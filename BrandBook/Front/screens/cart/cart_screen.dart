// // import 'package:flutter/material.dart';
// // import 'package:provider/provider.dart';
// // import 'cart_provider.dart';
// // import 'api_service.dart';
// // import 'package:shared_preferences/shared_preferences.dart';

// // class CartScreen extends StatelessWidget {
// //   const CartScreen({super.key});
// //   @override
// //   Widget build(BuildContext context) {
// //     final cart = Provider.of<CartProvider>(context);
// //     return Scaffold(
// //       appBar: AppBar(title: const Text('Корзина')),
// //       body: Column(
// //         children: [
// //           Expanded(
// //             child: ListView.builder(
// //               itemCount: cart.itemCount,
// //               itemBuilder: (context, index) {
// //                 final item = cart.items[index];
// //                 return Card(
// //                   margin: const EdgeInsets.symmetric(
// //                     horizontal: 16,
// //                     vertical: 8,
// //                   ),
// //                   child: ListTile(
// //                     title: Text(item.serviceName),
// //                     subtitle: Text(item.description),
// //                     trailing: Row(
// //                       mainAxisSize: MainAxisSize.min,
// //                       children: [
// //                         IconButton(
// //                           icon: const Icon(Icons.remove),
// //                           onPressed: () {
// //                             if (item.quantity > 1) {
// //                               cart.updateQuantity(
// //                                 item.serviceName,
// //                                 item.quantity - 1,
// //                               );
// //                             } else {
// //                               cart.removeItem(item.serviceName);
// //                             }
// //                           },
// //                         ),
// //                         Text(item.quantity.toString()),
// //                         IconButton(
// //                           icon: const Icon(Icons.add),
// //                           onPressed: () {
// //                             cart.updateQuantity(
// //                               item.serviceName,
// //                               item.quantity + 1,
// //                             );
// //                           },
// //                         ),
// //                         Text('${item.totalPrice.toStringAsFixed(2)} ₽'),
// //                       ],
// //                     ),
// //                   ),
// //                 );
// //               },
// //             ),
// //           ),
// //           Padding(
// //             padding: const EdgeInsets.all(16.0),
// //             child: Row(
// //               mainAxisAlignment: MainAxisAlignment.spaceBetween,
// //               children: [
// //                 Text(
// //                   'Итого: ${cart.totalPrice.toStringAsFixed(2)} ₽',
// //                   style: const TextStyle(
// //                     fontSize: 18,
// //                     fontWeight: FontWeight.bold,
// //                   ),
// //                 ),
// //                 ElevatedButton(
// //                   onPressed: () async {
// //                     final prefs = await SharedPreferences.getInstance();
// //                     final token = prefs.getString('auth_token') ?? '';
// //                     final apiService = ApiService();
// //                     bool allSuccess = true;
// //                     for (var item in cart.items) {
// //                       bool success = await apiService.addOrder(
// //                         token,
// //                         item.serviceName,
// //                         '${item.quantity} шт. — ${item.description}',
// //                       );
// //                       if (!success) allSuccess = false;
// //                     }
// //                     if (allSuccess) {
// //                       ScaffoldMessenger.of(context).showSnackBar(
// //                         const SnackBar(content: Text('Заказ оформлен!')),
// //                       );
// //                       cart.clearCart();
// //                     } else {
// //                       ScaffoldMessenger.of(context).showSnackBar(
// //                         const SnackBar(
// //                           content: Text('Ошибка при оформлении заказа'),
// //                         ),
// //                       );
// //                     }
// //                   },
// //                   child: const Text('Оформить заказ'),
// //                 ),
// //               ],
// //             ),
// //           ),
// //         ],
// //       ),
// //     );
// //   }
// // }

// // // import 'package:brandbook_app/screens/profile/login_screen.dart';
// // // import 'package:flutter/material.dart';
// // // import 'package:provider/provider.dart';
// // // import 'cart_provider.dart';
// // // import 'api_service.dart';
// // // import 'providers/auth_provider.dart';

// // // class CartScreen extends StatelessWidget {
// // //   const CartScreen({super.key});

// // //   @override
// // //   Widget build(BuildContext context) {
// // //     final auth = Provider.of<AuthProvider>(context);
// // //     final cart = Provider.of<CartProvider>(context);

// // //     // if (auth.isAuthenticated) {
// // //     //   return CartScreen();
// // //     // } else {
// // //     //   return LoginScreen();
// // //     // }

// // //     if (!auth.isAuthenticated) {
// // //       return Scaffold(
// // //         appBar: AppBar(title: const Text('Корзина')),
// // //         body: Center(
// // //           child: Column(
// // //             mainAxisSize: MainAxisSize.min,
// // //             children: [
// // //               const Text(
// // //                 'Войдите, чтобы использовать корзину и оформить заказ.',
// // //                 textAlign: TextAlign.center,
// // //               ),
// // //               const SizedBox(height: 16),
// // //               ElevatedButton(
// // //                 onPressed: () {
// // //                   Navigator.of(context).pushNamed('/login');
// // //                 },
// // //                 child: const Text('Войти'),
// // //               ),
// // //             ],
// // //           ),
// // //         ),
// // //       );
// // //     }

// // //     return Scaffold(
// // //       appBar: AppBar(title: const Text('Корзина')),
// // //       body: Column(
// // //         children: [
// // //           Expanded(
// // //             child: ListView.builder(
// // //               itemCount: cart.itemCount,
// // //               itemBuilder: (context, index) {
// // //                 final item = cart.items[index];
// // //                 return Card(
// // //                   margin: const EdgeInsets.symmetric(
// // //                     horizontal: 16,
// // //                     vertical: 8,
// // //                   ),
// // //                   child: ListTile(
// // //                     title: Text(item.serviceName),
// // //                     subtitle: Text(item.description),
// // //                     trailing: Row(
// // //                       mainAxisSize: MainAxisSize.min,
// // //                       children: [
// // //                         IconButton(
// // //                           icon: const Icon(Icons.remove),
// // //                           onPressed: () {
// // //                             if (item.quantity > 1) {
// // //                               cart.updateQuantity(
// // //                                 item.serviceName,
// // //                                 item.quantity - 1,
// // //                               );
// // //                             } else {
// // //                               cart.removeItem(item.serviceName);
// // //                             }
// // //                           },
// // //                         ),
// // //                         Text(item.quantity.toString()),
// // //                         IconButton(
// // //                           icon: const Icon(Icons.add),
// // //                           onPressed: () {
// // //                             cart.updateQuantity(
// // //                               item.serviceName,
// // //                               item.quantity + 1,
// // //                             );
// // //                           },
// // //                         ),
// // //                         Text('${item.totalPrice.toStringAsFixed(2)} ₽'),
// // //                       ],
// // //                     ),
// // //                   ),
// // //                 );
// // //               },
// // //             ),
// // //           ),
// // //           Padding(
// // //             padding: const EdgeInsets.all(16.0),
// // //             child: Row(
// // //               mainAxisAlignment: MainAxisAlignment.spaceBetween,
// // //               children: [
// // //                 Text(
// // //                   'Итого: ${cart.totalPrice.toStringAsFixed(2)} ₽',
// // //                   style: const TextStyle(
// // //                     fontSize: 18,
// // //                     fontWeight: FontWeight.bold,
// // //                   ),
// // //                 ),
// // //                 ElevatedButton(
// // //                   onPressed: () async {
// // //                     final token = auth.token;
// // //                     if (token == null || token.isEmpty) {
// // //                       ScaffoldMessenger.of(context).showSnackBar(
// // //                         const SnackBar(
// // //                           content: Text('Авторизуйтесь, чтобы оформить заказ'),
// // //                         ),
// // //                       );
// // //                       return;
// // //                     }

// // //                     final apiService = ApiService();
// // //                     bool allSuccess = true;

// // //                     for (var item in cart.items) {
// // //                       bool success = await apiService.addOrder(
// // //                         token,
// // //                         item.serviceName,
// // //                         '${item.quantity} шт. — ${item.description}',
// // //                       );
// // //                       if (!success) allSuccess = false;
// // //                     }

// // //                     if (allSuccess) {
// // //                       ScaffoldMessenger.of(context).showSnackBar(
// // //                         const SnackBar(content: Text('Заказ оформлен!')),
// // //                       );
// // //                       cart.clearCart();
// // //                     } else {
// // //                       ScaffoldMessenger.of(context).showSnackBar(
// // //                         const SnackBar(
// // //                           content: Text('Ошибка при оформлении заказа'),
// // //                         ),
// // //                       );
// // //                     }
// // //                   },
// // //                   child: const Text('Оформить заказ'),
// // //                 ),
// // //               ],
// // //             ),
// // //           ),
// // //         ],
// // //       ),
// // //     );
// // //   }
// // // }

// // //ГЛавный вариант
// // // import 'package:flutter/material.dart';
// // // import 'package:provider/provider.dart';
// // // import 'cart_provider.dart';
// // // import 'api_service.dart';
// // // import 'providers/auth_provider.dart';
// // // import '../profile/login_screen.dart';

// // // class CartScreen extends StatelessWidget {
// // //   const CartScreen({super.key});

// // //   @override
// // //   Widget build(BuildContext context) {
// // //     return Consumer<AuthProvider>(
// // //       builder: (context, auth, child) {
// // //         // Если пользователь не авторизован — показываем экран с кнопкой "Войти"
// // //         if (!auth.isAuthenticated) {
// // //           return Scaffold(
// // //             appBar: AppBar(title: const Text('Корзина')),
// // //             body: Center(
// // //               child: Column(
// // //                 mainAxisSize: MainAxisSize.min,
// // //                 children: [
// // //                   const Text(
// // //                     'Войдите, чтобы использовать корзину и оформить заказ.',
// // //                     textAlign: TextAlign.center,
// // //                   ),
// // //                   const SizedBox(height: 16),
// // //                   ElevatedButton(
// // //                     onPressed: () {
// // //                       Navigator.of(context).push(
// // //                         MaterialPageRoute(
// // //                           builder: (context) => const LoginScreen(),
// // //                         ),
// // //                       );
// // //                     },
// // //                     child: const Text('Войти'),
// // //                   ),
// // //                 ],
// // //               ),
// // //             ),
// // //           );
// // //         } else {
// // //           return CartContent();
// // //         }
// // //       },
// // //     );
// // //   }
// // // }

// // // class CartContent extends StatelessWidget {
// // //   const CartContent({super.key});

// // //   @override
// // //   Widget build(BuildContext context) {
// // //     final cart = Provider.of<CartProvider>(context);
// // //     final auth = Provider.of<AuthProvider>(context);

// // //     return Scaffold(
// // //       appBar: AppBar(title: const Text('Корзина')),
// // //       body: Column(
// // //         children: [
// // //           Expanded(
// // //             child: ListView.builder(
// // //               itemCount: cart.itemCount,
// // //               itemBuilder: (context, index) {
// // //                 final item = cart.items[index];
// // //                 return Card(
// // //                   margin: const EdgeInsets.symmetric(
// // //                     horizontal: 16,
// // //                     vertical: 8,
// // //                   ),
// // //                   child: ListTile(
// // //                     title: Text(item.serviceName),
// // //                     subtitle: Text(item.description),
// // //                     trailing: Row(
// // //                       mainAxisSize: MainAxisSize.min,
// // //                       children: [
// // //                         IconButton(
// // //                           icon: const Icon(Icons.remove),
// // //                           onPressed: () {
// // //                             if (item.quantity > 1) {
// // //                               cart.updateQuantity(
// // //                                 item.serviceName,
// // //                                 item.quantity - 1,
// // //                               );
// // //                             } else {
// // //                               cart.removeItem(item.serviceName);
// // //                             }
// // //                           },
// // //                         ),
// // //                         Text(item.quantity.toString()),
// // //                         IconButton(
// // //                           icon: const Icon(Icons.add),
// // //                           onPressed: () {
// // //                             cart.updateQuantity(
// // //                               item.serviceName,
// // //                               item.quantity + 1,
// // //                             );
// // //                           },
// // //                         ),
// // //                         Text('${item.totalPrice.toStringAsFixed(2)} ₽'),
// // //                       ],
// // //                     ),
// // //                   ),
// // //                 );
// // //               },
// // //             ),
// // //           ),
// // //           Padding(
// // //             padding: const EdgeInsets.all(16.0),
// // //             child: Row(
// // //               mainAxisAlignment: MainAxisAlignment.spaceBetween,
// // //               children: [
// // //                 Text(
// // //                   'Итого: ${cart.totalPrice.toStringAsFixed(2)} ₽',
// // //                   style: const TextStyle(
// // //                     fontSize: 18,
// // //                     fontWeight: FontWeight.bold,
// // //                   ),
// // //                 ),
// // //                 ElevatedButton(
// // //                   onPressed: () async {
// // //                     final token = auth.token;
// // //                     if (token == null || token.isEmpty) {
// // //                       ScaffoldMessenger.of(context).showSnackBar(
// // //                         const SnackBar(
// // //                           content: Text('Авторизуйтесь, чтобы оформить заказ'),
// // //                         ),
// // //                       );
// // //                       return;
// // //                     }

// // //                     final apiService = ApiService();
// // //                     bool allSuccess = true;

// // //                     for (var item in cart.items) {
// // //                       bool success = await apiService.addOrder(
// // //                         token,
// // //                         item.serviceName,
// // //                         '${item.quantity} шт. — ${item.description}',
// // //                       );
// // //                       if (!success) allSuccess = false;
// // //                     }

// // //                     if (allSuccess) {
// // //                       ScaffoldMessenger.of(context).showSnackBar(
// // //                         const SnackBar(content: Text('Заказ оформлен!')),
// // //                       );
// // //                       cart.clearCart();
// // //                     } else {
// // //                       ScaffoldMessenger.of(context).showSnackBar(
// // //                         const SnackBar(
// // //                           content: Text('Ошибка при оформлении заказа'),
// // //                         ),
// // //                       );
// // //                     }
// // //                   },
// // //                   child: const Text('Оформить заказ'),
// // //                 ),
// // //               ],
// // //             ),
// // //           ),
// // //         ],
// // //       ),
// // //     );
// // //   }
// // // }
// import 'package:flutter/material.dart';
// import 'package:provider/provider.dart';
// import 'cart_provider.dart';
// import 'api_service.dart';
// import 'providers/auth_provider.dart';
// import '../profile/login_screen.dart';

// class CartScreen extends StatelessWidget {
//   const CartScreen({super.key});

//   @override
//   Widget build(BuildContext context) {
//     return Consumer<AuthProvider>(
//       builder: (context, auth, child) {
//         if (!auth.isAuthenticated) {
//           // Пользователь не авторизован — показываем экран "Войти"
//           return Scaffold(
//             appBar: AppBar(title: const Text('Корзина')),
//             body: Center(
//               child: Column(
//                 mainAxisSize: MainAxisSize.min,
//                 children: [
//                   const Text(
//                     'Войдите, чтобы использовать корзину и оформить заказ.',
//                     textAlign: TextAlign.center,
//                   ),
//                   const SizedBox(height: 16),
//                   ElevatedButton(
//                     onPressed: () {
//                       Navigator.of(context).push(
//                         MaterialPageRoute(
//                           builder: (context) => const LoginScreen(),
//                         ),
//                       );
//                     },
//                     child: const Text('Войти'),
//                   ),
//                 ],
//               ),
//             ),
//           );
//         } else {
//           // Пользователь авторизован — показываем содержимое корзины
//           return const CartContent();
//         }
//       },
//     );
//   }
// }

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'cart_provider.dart';
import 'providers/auth_provider.dart';
import '../profile/login_screen.dart';
import 'api_service.dart';

class CartScreen extends StatelessWidget {
  const CartScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Consumer<AuthProvider>(
      builder: (context, auth, child) {
        if (!auth.isAuthenticated) {
          return Scaffold(
            appBar: AppBar(title: const Text('Корзина')),
            body: Center(
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Padding(
                    padding: EdgeInsets.symmetric(horizontal: 24.0),
                    child: Text(
                      'Войдите, чтобы использовать корзину и оформить заказ.',
                      textAlign: TextAlign.center,
                    ),
                  ),
                  const SizedBox(height: 16),
                  ElevatedButton(
                    onPressed: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(builder: (_) => const LoginScreen()),
                      );
                    },
                    child: const Text('Войти'),
                  ),
                ],
              ),
            ),
          );
        } else {
          // Пользователь авторизован — показываем контент корзины
          return CartContent();
        }
      },
    );
  }
}

class CartContent extends StatelessWidget {
  const CartContent({super.key});

  @override
  Widget build(BuildContext context) {
    // Отслеживаем и AuthProvider, и CartProvider
    final auth = Provider.of<AuthProvider>(context);
    final cart = Provider.of<CartProvider>(context);

    // // Если пользователь вышел, возвращаем экран "Войти"
    // if (!auth.isAuthenticated) {
    //   return const CartScreen();
    // }

    return Scaffold(
      appBar: AppBar(title: const Text('Корзина')),
      body: Column(
        children: [
          Expanded(
            child: ListView.builder(
              itemCount: cart.itemCount,
              itemBuilder: (context, index) {
                final item = cart.items[index];
                return Card(
                  margin: const EdgeInsets.symmetric(
                    horizontal: 16,
                    vertical: 8,
                  ),
                  child: ListTile(
                    title: Text(item.serviceName),
                    subtitle: Text(item.description),
                    trailing: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        IconButton(
                          icon: const Icon(Icons.remove),
                          onPressed: () {
                            if (item.quantity > 1) {
                              cart.updateQuantity(
                                item.serviceName,
                                item.quantity - 1,
                              );
                            } else {
                              cart.removeItem(item.serviceName);
                            }
                          },
                        ),
                        Text(item.quantity.toString()),
                        IconButton(
                          icon: const Icon(Icons.add),
                          onPressed: () {
                            cart.updateQuantity(
                              item.serviceName,
                              item.quantity + 1,
                            );
                          },
                        ),
                        Text('${item.totalPrice.toStringAsFixed(2)} ₽'),
                      ],
                    ),
                  ),
                );
              },
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  'Итого: ${cart.totalPrice.toStringAsFixed(2)} ₽',
                  style: const TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                ElevatedButton(
                  onPressed: () async {
                    final token = auth.token;
                    if (token == null || token.isEmpty) {
                      ScaffoldMessenger.of(context).showSnackBar(
                        const SnackBar(
                          content: Text('Авторизуйтесь, чтобы оформить заказ'),
                        ),
                      );
                      return;
                    }

                    final apiService = ApiService();
                    bool allSuccess = true;

                    for (var item in cart.items) {
                      bool success = await apiService.addOrder(
                        token,
                        item.serviceName,
                        '${item.quantity} шт. — ${item.description}',
                      );
                      if (!success) allSuccess = false;
                    }

                    if (allSuccess) {
                      ScaffoldMessenger.of(context).showSnackBar(
                        const SnackBar(content: Text('Заказ оформлен!')),
                      );
                      cart.clearCart();
                    } else {
                      ScaffoldMessenger.of(context).showSnackBar(
                        const SnackBar(
                          content: Text('Ошибка при оформлении заказа'),
                        ),
                      );
                    }
                  },
                  child: const Text('Оформить заказ'),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
