// import 'package:flutter/material.dart';
// import 'package:provider/provider.dart';
// import '../cart/cart_provider.dart';

// class AddToCartSection extends StatelessWidget {
//   final double? totalCost;
//   final String serviceName;
//   final String description;

//   const AddToCartSection({
//     super.key,
//     required this.totalCost,
//     required this.serviceName,
//     required this.description,
//   });

//   @override
//   Widget build(BuildContext context) {
//     if (totalCost == null) return const SizedBox.shrink();

//     return Column(
//       children: [
//         Text(
//           'Итоговая стоимость: ${totalCost!.toStringAsFixed(2)} ₽',
//           style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
//         ),
//         const SizedBox(height: 16),
//         ElevatedButton(
//           onPressed: () {
//             final cart = Provider.of<CartProvider>(context, listen: false);
//             cart.addItem(
//               CartItem(
//                 serviceName: serviceName,
//                 description: description,
//                 price: totalCost!,
//               ),
//             );
//             ScaffoldMessenger.of(context).showSnackBar(
//               const SnackBar(content: Text('Услуга добавлена в корзину')),
//             );
//           },
//           child: const Text('Добавить в корзину'),
//         ),
//       ],
//     );
//   }
// }
// // import 'package:flutter/material.dart';
// // import 'package:provider/provider.dart';
// // import 'cart_provider.dart';
// // import 'providers/auth_provider.dart';

// // class AddToCartSection extends StatelessWidget {
// //   final String serviceName;
// //   final String description;
// //   final double? totalCost;

// //   const AddToCartSection({
// //     super.key,
// //     required this.serviceName,
// //     required this.description,
// //     required this.totalCost,
// //   });

// //   @override
// //   Widget build(BuildContext context) {
// //     final auth = Provider.of<AuthProvider>(context, listen: false);
// //     final cart = Provider.of<CartProvider>(context, listen: false);

// //     return ElevatedButton(
// //       onPressed: () {
// //         if (!auth.isAuthenticated) {
// //           ScaffoldMessenger.of(context).showSnackBar(
// //             const SnackBar(content: Text('Войдите, чтобы добавить в корзину')),
// //           );
// //           Navigator.of(context).pushNamed('/login');
// //           return;
// //         }

// //         cart.addItem(
// //           CartItem(
// //             serviceName: serviceName,
// //             description: description,
// //             quantity: 1,
// //             price: totalCost,
// //           ),
// //         );

// //         ScaffoldMessenger.of(
// //           context,
// //         ).showSnackBar(const SnackBar(content: Text('Добавлено в корзину')));
// //       },
// //       child: const Text('Добавить в корзину'),
// //     );
// //   }
// // }
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../cart/cart_provider.dart';

class AddToCartSection extends StatelessWidget {
  final double? totalCost;
  final String serviceName;
  final String description;

  const AddToCartSection({
    super.key,
    required this.totalCost,
    required this.serviceName,
    required this.description,
  });

  @override
  Widget build(BuildContext context) {
    if (totalCost == null) return const SizedBox.shrink();

    return Column(
      children: [
        Text(
          'Итоговая стоимость: ${totalCost!.toStringAsFixed(2)} ₽',
          style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 16),
        ElevatedButton(
          onPressed: () {
            final cart = Provider.of<CartProvider>(context, listen: false);
            // Добавляем только если пользователь авторизован (CartProvider сам проверит токен)
            cart.addItem(
              CartItem(
                serviceName: serviceName,
                description: description,
                price: totalCost!,
              ),
            );
            ScaffoldMessenger.of(context).showSnackBar(
              const SnackBar(content: Text('Услуга добавлена в корзину')),
            );
          },
          child: const Text('Добавить в корзину'),
        ),
      ],
    );
  }
}
