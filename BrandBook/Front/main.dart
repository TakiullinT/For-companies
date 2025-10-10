// import 'package:flutter/material.dart';
// import 'package:provider/provider.dart';
// import 'package:brandbook_app/screens/home_screen.dart';
// import 'package:brandbook_app/screens/cart/cart_provider.dart';
// import 'package:intl/date_symbol_data_local.dart';

// void main() async {
//   WidgetsFlutterBinding.ensureInitialized();

//   await initializeDateFormatting('ru');

//   runApp(
//     ChangeNotifierProvider(
//       create: (_) => CartProvider(),
//       child: const BrandbookApp(),
//     ),
//   );
// }

// class BrandbookApp extends StatelessWidget {
//   const BrandbookApp({super.key});

//   @override
//   Widget build(BuildContext context) {
//     return MaterialApp(
//       title: 'Brandbook',
//       debugShowCheckedModeBanner: false,
//       theme: ThemeData(
//         scaffoldBackgroundColor: const Color(0xFFF5F5F5), // общий фон
//         primaryColor: const Color(0xFF000000), // чёрный
//         colorScheme: ColorScheme.light(
//           primary: const Color(0xFF000000), // чёрный
//           secondary: const Color(0xFFFFD700), // золотой
//           surface: const Color(0xFFFFFFFF), // белый

//           background: const Color(0xFFF5F5F5), // светло‑серый
//           onPrimary: const Color(0xFFFFFFFF), // текст на чёрном фоне
//           onSecondary: const Color(0xFF000000), // текст на золотом фоне
//           onSurface: const Color(0xFF333333), // основной текст
//         ),
//         appBarTheme: const AppBarTheme(
//           backgroundColor: Color(0xFF000000),
//           iconTheme: IconThemeData(color: Color(0xFFFFD700)),
//           titleTextStyle: TextStyle(
//             color: Colors.white,
//             fontSize: 20,
//             fontWeight: FontWeight.w500,
//           ),
//         ),
//         elevatedButtonTheme: ElevatedButtonThemeData(
//           style: ElevatedButton.styleFrom(
//             backgroundColor: const Color(0xFFFFD700),
//             foregroundColor: const Color(0xFF000000),
//             padding: const EdgeInsets.symmetric(vertical: 14),
//             shape: RoundedRectangleBorder(
//               borderRadius: BorderRadius.circular(5),
//             ),
//           ),
//         ),
//         textTheme: const TextTheme(
//           bodyLarge: TextStyle(color: Color(0xFF333333)),
//           headlineMedium: TextStyle(color: Color(0xFF000000)),
//         ),
//       ),
//       home: const HomeScreen(),
//     );
//   }
// }
import 'package:brandbook_app/screens/cart/cart_screen.dart';
import 'package:brandbook_app/screens/profile/login_screen.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:brandbook_app/screens/home_screen.dart';
import 'package:brandbook_app/screens/cart/cart_provider.dart';
import 'package:brandbook_app/screens/cart/providers/auth_provider.dart'; // <-- добавь импорт
import 'package:intl/date_symbol_data_local.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  await initializeDateFormatting('ru');

  runApp(
    // MultiProvider(
    //   providers: [
    //     ChangeNotifierProvider(create: (_) => CartProvider()),
    //     ChangeNotifierProvider(create: (_) => AuthProvider()), // <-- вот он
    //   ],
    //   child: const BrandbookApp(),
    // ),
    MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => AuthProvider()),
        ChangeNotifierProxyProvider<AuthProvider, CartProvider>(
          create: (_) => CartProvider(),
          update: (_, auth, cart) {
            cart ??= CartProvider();
            cart.setAuthToken(auth.token);
            return cart;
          },
        ),
      ],
      child: const BrandbookApp(),
    ),
  );
}

class BrandbookApp extends StatelessWidget {
  const BrandbookApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Brandbook',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        scaffoldBackgroundColor: const Color(0xFFF5F5F5),
        primaryColor: const Color(0xFF000000),
        colorScheme: ColorScheme.light(
          primary: const Color(0xFF000000),
          secondary: const Color(0xFFFFD700),
          surface: const Color(0xFFFFFFFF),
          background: const Color(0xFFF5F5F5),
          onPrimary: const Color(0xFFFFFFFF),
          onSecondary: const Color(0xFF000000),
          onSurface: const Color(0xFF333333),
        ),
        appBarTheme: const AppBarTheme(
          backgroundColor: Color(0xFF000000),
          iconTheme: IconThemeData(color: Color(0xFFFFD700)),
          titleTextStyle: TextStyle(
            color: Colors.white,
            fontSize: 20,
            fontWeight: FontWeight.w500,
          ),
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: const Color(0xFFFFD700),
            foregroundColor: const Color(0xFF000000),
            padding: const EdgeInsets.symmetric(vertical: 14),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(5),
            ),
          ),
        ),
        textTheme: const TextTheme(
          bodyLarge: TextStyle(color: Color(0xFF333333)),
          headlineMedium: TextStyle(color: Color(0xFF000000)),
        ),
      ),
      home: const HomeScreen(),
      routes: {
        '/login': (context) => const LoginScreen(),
        '/cart': (context) => const CartScreen(),
      },
    );
  }
}
