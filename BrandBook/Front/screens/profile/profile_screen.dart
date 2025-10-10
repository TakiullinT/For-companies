// // import 'package:flutter/material.dart';
// // import 'package:shared_preferences/shared_preferences.dart';
// // import 'login_screen.dart';
// // import 'order_history_screen.dart';
// // import 'package:http/http.dart' as http;
// // import 'dart:convert';

// // class PersonalAccountScreen extends StatefulWidget {
// //   const PersonalAccountScreen({super.key});

// //   @override
// //   State<PersonalAccountScreen> createState() => _PersonalAccountScreenState();
// // }

// // class _PersonalAccountScreenState extends State<PersonalAccountScreen> {
// //   String? userName;
// //   String? userEmail;

// //   @override
// //   void initState() {
// //     super.initState();
// //     _loadProfile();
// //     _checkAuth();
// //   }

// //   Future<void> _checkAuth() async {
// //     final prefs = await SharedPreferences.getInstance();
// //     final token = prefs.getString('auth_token');

// //     if (token == null && mounted) {
// //       Navigator.pushNamed(context, '/login');
// //     }
// //   }

// //   Future<void> _loadProfile() async {
// //     final prefs = await SharedPreferences.getInstance();
// //     setState(() {
// //       userName = prefs.getString('user_name');
// //       userEmail = prefs.getString('user_email');
// //     });
// //   }

// //   Future<void> _updateProfile(String name, String email) async {
// //     final prefs = await SharedPreferences.getInstance();
// //     await prefs.setString('user_name', name);
// //     await prefs.setString('user_email', email);

// //     setState(() {
// //       userName = name;
// //       userEmail = email;
// //     });
// //   }

// //   Future<bool> updateProfileOnServer(String name, String email) async {
// //     final prefs = await SharedPreferences.getInstance();
// //     final token = prefs.getString('auth_token');

// //     if (token == null) {
// //       if (mounted) {
// //         ScaffoldMessenger.of(
// //           context,
// //         ).showSnackBar(const SnackBar(content: Text('Требуется авторизация')));
// //       }
// //       return false;
// //     }

// //     final response = await http.patch(
// //       Uri.parse('http://127.0.0.1:8000/api/profile/update/'),
// //       headers: {
// //         'Authorization': 'Token $token',
// //         'Content-Type': 'application/json',
// //       },
// //       body: jsonEncode({'name': name, 'email': email}),
// //     );

// //     return response.statusCode == 200;
// //   }

// //   Future<void> _logout() async {
// //     final prefs = await SharedPreferences.getInstance();
// //     await prefs.remove('user_name');
// //     await prefs.remove('user_email');

// //     setState(() {
// //       userName = null;
// //       userEmail = null;
// //     });

// //     if (context.mounted) {
// //       ScaffoldMessenger.of(
// //         context,
// //       ).showSnackBar(const SnackBar(content: Text('Вы вышли из аккаунта')));
// //     }
// //   }

// //   @override
// //   Widget build(BuildContext context) {
// //     return Scaffold(
// //       appBar: AppBar(title: const Text('Личный кабинет')),
// //       body: Padding(
// //         padding: const EdgeInsets.all(24),
// //         child: _buildProfile(context),
// //       ),
// //     );
// //   }

// //   Widget _buildProfile(BuildContext context) {
// //     final isLoggedIn = userName != null && userEmail != null;

// //     return Column(
// //       crossAxisAlignment: CrossAxisAlignment.center,
// //       children: [
// //         Center(
// //           child: Column(
// //             children: [
// //               CircleAvatar(
// //                 radius: 50,
// //                 backgroundImage: const AssetImage('assets/images/avatar.png'),
// //                 onBackgroundImageError: (_, __) {},
// //                 child: Image.asset(
// //                   'assets/images/avatar.png',
// //                   errorBuilder: (context, error, stackTrace) {
// //                     return const Icon(
// //                       Icons.account_circle,
// //                       size: 100,
// //                       color: Color(0xFFFFD700),
// //                     );
// //                   },
// //                 ),
// //               ),
// //               const SizedBox(height: 16),
// //               Text(
// //                 userName ?? 'Имя пользователя',
// //                 style: const TextStyle(
// //                   fontSize: 24,
// //                   fontWeight: FontWeight.bold,
// //                 ),
// //                 textAlign: TextAlign.center,
// //               ),
// //               const SizedBox(height: 8),
// //               Text(
// //                 userEmail ?? 'example@example.com',
// //                 style: const TextStyle(fontSize: 16, color: Colors.grey),
// //                 textAlign: TextAlign.center,
// //               ),
// //             ],
// //           ),
// //         ),
// //         const SizedBox(height: 32),

// //         if (isLoggedIn) ...[
// //           ElevatedButton.icon(
// //             icon: const Icon(Icons.history),
// //             label: const Text('История заказов'),
// //             onPressed: () {
// //               Navigator.push(
// //                 context,
// //                 MaterialPageRoute(builder: (_) => const OrderHistoryScreen()),
// //               );
// //             },
// //           ),
// //           const SizedBox(height: 16),

// //           ElevatedButton.icon(
// //             icon: const Icon(Icons.edit),
// //             label: const Text('Редактировать профиль'),
// //             style: ElevatedButton.styleFrom(
// //               foregroundColor: Colors.black,
// //               backgroundColor: Colors.grey.shade500,
// //             ),
// //             onPressed: () async {
// //               final result = await Navigator.push(
// //                 context,
// //                 MaterialPageRoute(
// //                   builder: (_) => EditProfileScreen(
// //                     initialName: userName!,
// //                     initialEmail: userEmail!,
// //                   ),
// //                 ),
// //               );

// //               if (result != null && result is Map<String, String>) {
// //                 await _updateProfile(result['name']!, result['email']!);
// //               }
// //             },
// //           ),
// //           const SizedBox(height: 16),

// //           ElevatedButton.icon(
// //             icon: Icon(Icons.logout),
// //             label: const Text('Выйти из аккаунта'),
// //             style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
// //             onPressed: () async {
// //               await _logout();
// //             },
// //           ),
// //         ] else ...[
// //           ElevatedButton.icon(
// //             icon: const Icon(Icons.login),
// //             label: const Text('Войти в аккаунт'),
// //             style: ElevatedButton.styleFrom(backgroundColor: Color(0xFFFFD700)),
// //             onPressed: () async {
// //               final result = await Navigator.push(
// //                 context,
// //                 MaterialPageRoute(builder: (_) => const LoginScreen()),
// //               );
// //               if (result == true) {
// //                 await _loadProfile();
// //               }
// //             },
// //           ),
// //         ],
// //       ],
// //     );
// //   }
// // }

// // class EditProfileScreen extends StatefulWidget {
// //   final String initialName;
// //   final String initialEmail;

// //   const EditProfileScreen({
// //     super.key,
// //     required this.initialName,
// //     required this.initialEmail,
// //   });

// //   @override
// //   State<EditProfileScreen> createState() => _EditProfileScreenState();
// // }

// // class _EditProfileScreenState extends State<EditProfileScreen> {
// //   late TextEditingController nameController;
// //   late TextEditingController emailController;
// //   final _formKey = GlobalKey<FormState>();
// //   bool _hasChanged = false;

// //   @override
// //   void initState() {
// //     super.initState();
// //     nameController = TextEditingController(text: widget.initialName);
// //     emailController = TextEditingController(text: widget.initialEmail);

// //     nameController.addListener(_onChanged);
// //     emailController.addListener(_onChanged);
// //   }

// //   void _onChanged() {
// //     final changed =
// //         nameController.text != widget.initialName ||
// //         emailController.text != widget.initialEmail;
// //     if (changed != _hasChanged) {
// //       setState(() {
// //         _hasChanged = changed;
// //       });
// //     }
// //   }

// //   void _save() {
// //     if (_formKey.currentState!.validate()) {
// //       Navigator.pop(context, {
// //         'name': nameController.text.trim(),
// //         'email': emailController.text.trim(),
// //       });
// //     }
// //   }

// //   @override
// //   void dispose() {
// //     nameController.removeListener(_onChanged);
// //     emailController.removeListener(_onChanged);
// //     nameController.dispose();
// //     emailController.dispose();
// //     super.dispose();
// //   }

// //   @override
// //   Widget build(BuildContext context) {
// //     return Scaffold(
// //       appBar: AppBar(title: const Text('Редактировать профиль')),
// //       body: Padding(
// //         padding: const EdgeInsets.all(24),
// //         child: Form(
// //           key: _formKey,
// //           child: Column(
// //             children: [
// //               TextFormField(
// //                 controller: nameController,
// //                 decoration: const InputDecoration(
// //                   labelText: 'Имя',
// //                   border: OutlineInputBorder(),
// //                 ),
// //                 validator: (value) => (value == null || value.trim().isEmpty)
// //                     ? 'Введите имя'
// //                     : null,
// //               ),
// //               const SizedBox(height: 16),
// //               TextFormField(
// //                 controller: emailController,
// //                 decoration: const InputDecoration(
// //                   labelText: 'Email',
// //                   border: OutlineInputBorder(),
// //                 ),
// //                 keyboardType: TextInputType.emailAddress,
// //                 validator: (value) {
// //                   if (value == null || value.trim().isEmpty)
// //                     return 'Введите email';
// //                   final emailRegex = RegExp(r'^[^@]+@[^@]+\.[^@]+');
// //                   if (!emailRegex.hasMatch(value.trim()))
// //                     return 'Введите корректный email';
// //                   return null;
// //                 },
// //               ),
// //               const Spacer(),
// //               if (_hasChanged)
// //                 SizedBox(
// //                   width: double.infinity,
// //                   child: ElevatedButton(
// //                     onPressed: _save,
// //                     child: const Text('Сохранить изменения'),
// //                   ),
// //                 ),
// //             ],
// //           ),
// //         ),
// //       ),
// //     );
// //   }
// // }
// import 'package:flutter/material.dart';
// import 'package:shared_preferences/shared_preferences.dart';
// import 'login_screen.dart';
// import 'order_history_screen.dart';
// import 'package:http/http.dart' as http;
// import 'dart:convert';
// import 'dart:io';
// import 'package:image_picker/image_picker.dart';
// import 'package:provider/provider.dart';
// import '../cart/providers/auth_provider.dart';

// class PersonalAccountScreen extends StatefulWidget {
//   const PersonalAccountScreen({super.key});

//   @override
//   State<PersonalAccountScreen> createState() => _PersonalAccountScreenState();
// }

// class _PersonalAccountScreenState extends State<PersonalAccountScreen> {
//   String? userName;
//   String? userEmail;

//   @override
//   void initState() {
//     super.initState();
//     _loadProfile(); // загружаем профиль и одновременно проверяем токен внутри
//   }

//   // Загружаем профиль и проверяем токен.
//   // Если токена нет — очищаем локальные поля (и экран покажет незалогиненный вид).
//   Future<void> _loadProfile() async {
//     final prefs = await SharedPreferences.getInstance();
//     final token = prefs.getString('auth_token');

//     setState(() {
//       if (token == null) {
//         // нет токена — считаем, что пользователь неавторизован
//         userName = null;
//         userEmail = null;
//       } else {
//         // есть токен — загружаем данные (если есть)
//         userName = prefs.getString('user_name');
//         userEmail = prefs.getString('user_email');
//       }
//     });
//   }

//   Future<void> _updateProfile(String name, String email) async {
//     final prefs = await SharedPreferences.getInstance();
//     await prefs.setString('user_name', name);
//     await prefs.setString('user_email', email);

//     setState(() {
//       userName = name;
//       userEmail = email;
//     });
//   }

//   Future<bool> updateProfileOnServer(String name, String email) async {
//     final prefs = await SharedPreferences.getInstance();
//     final token = prefs.getString('auth_token');

//     if (token == null) {
//       if (mounted) {
//         ScaffoldMessenger.of(
//           context,
//         ).showSnackBar(const SnackBar(content: Text('Требуется авторизация')));
//       }
//       return false;
//     }

//     final response = await http.patch(
//       Uri.parse('http://127.0.0.1:8000/api/profile/update/'),
//       headers: {
//         'Authorization': 'Token $token',
//         'Content-Type': 'application/json',
//       },
//       body: jsonEncode({'name': name, 'email': email}),
//     );

//     return response.statusCode == 200;
//   }

//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(title: const Text('Личный кабинет')),
//       body: Padding(
//         padding: const EdgeInsets.all(24),
//         child: _buildProfile(context),
//       ),
//     );
//   }

//   Widget _buildProfile(BuildContext context) {
//     final isLoggedIn = userName != null && userEmail != null;

//     return Column(
//       crossAxisAlignment: CrossAxisAlignment.center,
//       children: [
//         Center(
//           child: Column(
//             children: [
//               CircleAvatar(
//                 radius: 50,
//                 backgroundImage: const AssetImage('assets/images/avatar.png'),
//                 onBackgroundImageError: (_, __) {},
//                 child: Image.asset(
//                   'assets/images/avatar.png',
//                   errorBuilder: (context, error, stackTrace) {
//                     return const Icon(
//                       Icons.account_circle,
//                       size: 100,
//                       color: Color(0xFFFFD700),
//                     );
//                   },
//                 ),
//               ),
//               const SizedBox(height: 16),
//               Text(
//                 userName ?? 'Имя пользователя',
//                 style: const TextStyle(
//                   fontSize: 24,
//                   fontWeight: FontWeight.bold,
//                 ),
//                 textAlign: TextAlign.center,
//               ),
//               const SizedBox(height: 8),
//               Text(
//                 userEmail ?? 'example@example.com',
//                 style: const TextStyle(fontSize: 16, color: Colors.grey),
//                 textAlign: TextAlign.center,
//               ),
//             ],
//           ),
//         ),
//         const SizedBox(height: 32),

//         if (isLoggedIn) ...[
//           ElevatedButton.icon(
//             icon: const Icon(Icons.history),
//             label: const Text('История заказов'),
//             onPressed: () {
//               Navigator.push(
//                 context,
//                 MaterialPageRoute(builder: (_) => const OrderHistoryScreen()),
//               );
//             },
//           ),
//           const SizedBox(height: 16),

//           ElevatedButton.icon(
//             icon: const Icon(Icons.edit),
//             label: const Text('Редактировать профиль'),
//             style: ElevatedButton.styleFrom(
//               foregroundColor: Colors.black,
//               backgroundColor: Colors.grey.shade500,
//             ),
//             onPressed: () async {
//               final result = await Navigator.push(
//                 context,
//                 MaterialPageRoute(
//                   builder: (_) => EditProfileScreen(
//                     initialName: userName!,
//                     initialEmail: userEmail!,
//                   ),
//                 ),
//               );

//               if (result != null && result is Map<String, String>) {
//                 await _updateProfile(result['name']!, result['email']!);
//               }
//             },
//           ),
//           const SizedBox(height: 16),

//           ElevatedButton.icon(
//             icon: const Icon(Icons.logout),
//             label: const Text('Выйти из аккаунта'),
//             style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
//             onPressed: () async {
//               // Вызываем logout у AuthProvider — это очистит токен, SharedPreferences и вызовет notifyListeners()
//               final auth = Provider.of<AuthProvider>(context, listen: false);
//               await auth.logout();

//               // Обновим локальный state, чтобы экран сразу показал незалогиненный вид.
//               if (mounted) {
//                 setState(() {
//                   userName = null;
//                   userEmail = null;
//                 });
//               }

//               // НЕ делаем автоматического навигационного редиректа — пользователь остаётся на этом экране
//             },
//           ),
//         ] else ...[
//           ElevatedButton.icon(
//             icon: const Icon(Icons.login),
//             label: const Text('Войти в аккаунт'),
//             style: ElevatedButton.styleFrom(backgroundColor: Color(0xFFFFD700)),
//             onPressed: () async {
//               // Переходим на экран логина; LoginScreen должен возвращать true при успешном логине
//               final result = await Navigator.push(
//                 context,
//                 MaterialPageRoute(builder: (_) => const LoginScreen()),
//               );
//               if (result == true) {
//                 // После успешного логина заново подгружаем профиль (теперь в SharedPreferences появится токен)
//                 await _loadProfile();
//               }
//             },
//           ),
//         ],
//       ],
//     );
//   }
// }

// class EditProfileScreen extends StatefulWidget {
//   final String initialName;
//   final String initialEmail;

//   const EditProfileScreen({
//     super.key,
//     required this.initialName,
//     required this.initialEmail,
//   });

//   @override
//   State<EditProfileScreen> createState() => _EditProfileScreenState();
// }

// class _EditProfileScreenState extends State<EditProfileScreen> {
//   late TextEditingController nameController;
//   late TextEditingController emailController;
//   final _formKey = GlobalKey<FormState>();
//   bool _hasChanged = false;

//   @override
//   void initState() {
//     super.initState();
//     nameController = TextEditingController(text: widget.initialName);
//     emailController = TextEditingController(text: widget.initialEmail);

//     nameController.addListener(_onChanged);
//     emailController.addListener(_onChanged);
//   }

//   void _onChanged() {
//     final changed =
//         nameController.text != widget.initialName ||
//         emailController.text != widget.initialEmail;
//     if (changed != _hasChanged) {
//       setState(() {
//         _hasChanged = changed;
//       });
//     }
//   }

//   void _save() {
//     if (_formKey.currentState!.validate()) {
//       Navigator.pop(context, {
//         'name': nameController.text.trim(),
//         'email': emailController.text.trim(),
//       });
//     }
//   }

//   @override
//   void dispose() {
//     nameController.removeListener(_onChanged);
//     emailController.removeListener(_onChanged);
//     nameController.dispose();
//     emailController.dispose();
//     super.dispose();
//   }

//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(title: const Text('Редактировать профиль')),
//       body: Padding(
//         padding: const EdgeInsets.all(24),
//         child: Form(
//           key: _formKey,
//           child: Column(
//             children: [
//               TextFormField(
//                 controller: nameController,
//                 decoration: const InputDecoration(
//                   labelText: 'Имя',
//                   border: OutlineInputBorder(),
//                 ),
//                 validator: (value) => (value == null || value.trim().isEmpty)
//                     ? 'Введите имя'
//                     : null,
//               ),
//               const SizedBox(height: 16),
//               TextFormField(
//                 controller: emailController,
//                 decoration: const InputDecoration(
//                   labelText: 'Email',
//                   border: OutlineInputBorder(),
//                 ),
//                 keyboardType: TextInputType.emailAddress,
//                 validator: (value) {
//                   if (value == null || value.trim().isEmpty)
//                     return 'Введите email';
//                   final emailRegex = RegExp(r'^[^@]+@[^@]+\.[^@]+');
//                   if (!emailRegex.hasMatch(value.trim()))
//                     return 'Введите корректный email';
//                   return null;
//                 },
//               ),
//               const Spacer(),
//               if (_hasChanged)
//                 SizedBox(
//                   width: double.infinity,
//                   child: ElevatedButton(
//                     onPressed: _save,
//                     child: const Text('Сохранить изменения'),
//                   ),
//                 ),
//             ],
//           ),
//         ),
//       ),
//     );
//   }
// }

import 'dart:io';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'login_screen.dart';
import 'order_history_screen.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:provider/provider.dart';
import 'package:image_picker/image_picker.dart';
import '../cart/providers/auth_provider.dart';

class PersonalAccountScreen extends StatefulWidget {
  const PersonalAccountScreen({super.key});

  @override
  State<PersonalAccountScreen> createState() => _PersonalAccountScreenState();
}

class _PersonalAccountScreenState extends State<PersonalAccountScreen> {
  String? userName;
  String? userEmail;
  File? _avatarFile;

  // @override
  // void initState() {
  //   super.initState();
  //   _loadProfile();
  //   _loadAvatar();
  // }
  @override
  void initState() {
    super.initState();
    final auth = Provider.of<AuthProvider>(context, listen: false);
    userName = auth.name;
    userEmail = auth.email;
    _loadAvatar();
  }

  Future<void> _loadProfile() async {
    final prefs = await SharedPreferences.getInstance();
    setState(() {
      userName = prefs.getString('user_name');
      userEmail = prefs.getString('user_email');
    });
  }

  Future<void> _loadAvatar() async {
    final prefs = await SharedPreferences.getInstance();
    final path = prefs.getString('user_avatar');
    if (path != null && File(path).existsSync()) {
      setState(() {
        _avatarFile = File(path);
      });
    }
  }

  Future<void> _pickAvatar() async {
    final picker = ImagePicker();
    final pickedFile = await picker.pickImage(source: ImageSource.gallery);

    if (pickedFile != null) {
      setState(() {
        _avatarFile = File(pickedFile.path);
      });

      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('user_avatar', pickedFile.path);
    }
  }

  Future<void> _updateProfile(String name, String email) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('user_name', name);
    await prefs.setString('user_email', email);

    setState(() {
      userName = name;
      userEmail = email;
    });
  }

  Future<bool> updateProfileOnServer(String name, String email) async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('auth_token');

    if (token == null) {
      if (mounted) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(const SnackBar(content: Text('Требуется авторизация')));
      }
      return false;
    }

    final response = await http.patch(
      Uri.parse('http://127.0.0.1:8000/api/profile/update/'),
      headers: {
        'Authorization': 'Token $token',
        'Content-Type': 'application/json',
      },
      body: jsonEncode({'name': name, 'email': email}),
    );

    return response.statusCode == 200;
  }

  @override
  Widget build(BuildContext context) {
    final isLoggedIn = userName != null && userEmail != null;

    return Scaffold(
      appBar: AppBar(title: const Text('Личный кабинет')),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Center(
              child: GestureDetector(
                onTap: isLoggedIn ? _pickAvatar : null,
                child: CircleAvatar(
                  radius: 50,
                  backgroundImage: _avatarFile != null
                      ? FileImage(_avatarFile!)
                      : const AssetImage('assets/images/avatar.png')
                            as ImageProvider,
                  child: _avatarFile == null
                      ? const Icon(
                          Icons.camera_alt,
                          size: 32,
                          color: Colors.white70,
                        )
                      : null,
                ),
              ),
            ),
            const SizedBox(height: 16),
            Text(
              userName ?? 'Имя пользователя',
              style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 8),
            Text(
              userEmail ?? 'example@example.com',
              style: const TextStyle(fontSize: 16, color: Colors.grey),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 32),

            if (isLoggedIn) ...[
              ElevatedButton.icon(
                icon: const Icon(Icons.history),
                label: const Text('История заказов'),
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (_) => const OrderHistoryScreen(),
                    ),
                  );
                },
              ),
              const SizedBox(height: 16),

              ElevatedButton.icon(
                icon: const Icon(Icons.edit),
                label: const Text('Редактировать профиль'),
                style: ElevatedButton.styleFrom(
                  foregroundColor: Colors.black,
                  backgroundColor: Colors.grey.shade500,
                ),
                onPressed: () async {
                  final result = await Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (_) => EditProfileScreen(
                        initialName: userName!,
                        initialEmail: userEmail!,
                      ),
                    ),
                  );

                  if (result != null && result is Map<String, String>) {
                    await _updateProfile(result['name']!, result['email']!);
                  }
                },
              ),
              const SizedBox(height: 16),

              ElevatedButton.icon(
                icon: const Icon(Icons.logout),
                label: const Text('Выйти из аккаунта'),
                style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
                onPressed: () async {
                  final auth = Provider.of<AuthProvider>(
                    context,
                    listen: false,
                  );
                  await auth.logout();
                  if (mounted) {
                    setState(() {
                      userName = null;
                      userEmail = null;
                      _avatarFile = null;
                    });
                  }
                },
              ),
            ] else ...[
              ElevatedButton.icon(
                icon: const Icon(Icons.login),
                label: const Text('Войти в аккаунт'),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Color(0xFFFFD700),
                ),
                onPressed: () async {
                  final result = await Navigator.push(
                    context,
                    MaterialPageRoute(builder: (_) => const LoginScreen()),
                  );
                  if (result == true) {
                    await _loadProfile();
                    await _loadAvatar();
                  }
                },
              ),
            ],
          ],
        ),
      ),
    );
  }
}

class EditProfileScreen extends StatefulWidget {
  final String initialName;
  final String initialEmail;

  const EditProfileScreen({
    super.key,
    required this.initialName,
    required this.initialEmail,
  });

  @override
  State<EditProfileScreen> createState() => _EditProfileScreenState();
}

class _EditProfileScreenState extends State<EditProfileScreen> {
  late TextEditingController nameController;
  late TextEditingController emailController;
  final _formKey = GlobalKey<FormState>();
  bool _hasChanged = false;

  @override
  void initState() {
    super.initState();
    nameController = TextEditingController(text: widget.initialName);
    emailController = TextEditingController(text: widget.initialEmail);

    nameController.addListener(_onChanged);
    emailController.addListener(_onChanged);
  }

  void _onChanged() {
    final changed =
        nameController.text != widget.initialName ||
        emailController.text != widget.initialEmail;
    if (changed != _hasChanged) {
      setState(() {
        _hasChanged = changed;
      });
    }
  }

  void _save() {
    if (_formKey.currentState!.validate()) {
      Navigator.pop(context, {
        'name': nameController.text.trim(),
        'email': emailController.text.trim(),
      });
    }
  }

  @override
  void dispose() {
    nameController.removeListener(_onChanged);
    emailController.removeListener(_onChanged);
    nameController.dispose();
    emailController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Редактировать профиль')),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                controller: nameController,
                decoration: const InputDecoration(
                  labelText: 'Имя',
                  border: OutlineInputBorder(),
                ),
                validator: (value) => (value == null || value.trim().isEmpty)
                    ? 'Введите имя'
                    : null,
              ),
              const SizedBox(height: 16),
              TextFormField(
                controller: emailController,
                decoration: const InputDecoration(
                  labelText: 'Email',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.emailAddress,
                validator: (value) {
                  if (value == null || value.trim().isEmpty)
                    return 'Введите email';
                  final emailRegex = RegExp(r'^[^@]+@[^@]+\.[^@]+');
                  if (!emailRegex.hasMatch(value.trim()))
                    return 'Введите корректный email';
                  return null;
                },
              ),
              const Spacer(),
              if (_hasChanged)
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton(
                    onPressed: _save,
                    child: const Text('Сохранить изменения'),
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}
