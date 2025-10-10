// import 'dart:convert';
// import 'package:flutter/material.dart';
// import 'package:uuid/uuid.dart';
// import 'package:web_socket_channel/web_socket_channel.dart';
// import 'package:shared_preferences/shared_preferences.dart';
// import 'package:brandbook_app/screens/cart/providers/auth_provider.dart';
// import 'package:provider/provider.dart';
// import 'profile/login_screen.dart';

// class ChatWithManagerScreen extends StatefulWidget {
//   final String managerName;
//   const ChatWithManagerScreen({super.key, required this.managerName});

//   @override
//   State<ChatWithManagerScreen> createState() => _ChatWithManagerScreen();
// }

// class _ChatWithManagerScreen extends State<ChatWithManagerScreen> {
//   final List<_Message> _messages = [];
//   final TextEditingController _controller = TextEditingController();
//   final ScrollController _scrollController = ScrollController();
//   late final WebSocketChannel _channel;
//   final _uuid = const Uuid();
//   final String mySenderId = 'me';
//   static const String _cacheKeyPrefix = 'chat_cache_';

//   @override
//   void initState() {
//     super.initState();
//     _loadCache().then((_) {
//       _connect();
//     });
//   }

//   // Future<void> _checkAuthAndLoad() async {
//   //   final authProvider = Provider.of<AuthProvider>(context, listen: false);
//   //   if (!authProvider.isAuthenticated) {
//   //     // Redirect to login screen if not authenticated
//   //     Navigator.pushReplacement(
//   //       context,
//   //       MaterialPageRoute(builder: (_) => const LoginScreen()),
//   //     );
//   //     return;
//   //   }
//   //   await _loadCache();
//   //   _connect();
//   // }

//   void _connect() {
//     //final authProvider = Provider.of<AuthProvider>(context, listen: false);
//     final role = mySenderId;
//     // final token = authProvider.token;
//     // final role = authProvider.name ?? 'me';
//     _channel = WebSocketChannel.connect(
//       Uri.parse('ws://127.0.0.1:8000/ws/chat/testroom/?sender=$role'),
//     );

//     _channel.stream.listen(
//       (data) {
//         final map = jsonDecode(data);
//         final type = map['type'] ?? 'message';

//         if (type == 'history') {
//           final List msgs = map['messages'] ?? [];
//           setState(() {
//             _messages.clear();
//             for (final m in msgs) {
//               _messages.add(
//                 _Message(
//                   id: m['id'],
//                   sender: m['sender'],
//                   text: m['message'],
//                   timestamp: DateTime.tryParse(m['timestamp'] ?? ''),
//                   isPending: false,
//                 ),
//               );
//             }
//           });
//           _saveCache();
//           _scrollToBottom();
//           return;
//         }

//         if (type == 'message') {
//           final msgId = map['id'] ?? _uuid.v4();
//           final sender = map['sender'] ?? 'unknown';
//           final text = map['message'] ?? '';
//           final timestamp = DateTime.tryParse(map['timestamp'] ?? '');

//           if (sender == mySenderId) {
//             setState(() {
//               final idx = _messages.indexWhere((m) => m.id == msgId);
//               if (idx != -1) {
//                 _messages[idx] = _messages[idx].copyWith(
//                   isPending: true,
//                   timestamp: timestamp ?? _messages[idx].timestamp,
//                 );
//               } else {
//                 _messages.add(
//                   _Message(
//                     id: msgId,
//                     sender: sender,
//                     text: text,
//                     timestamp: timestamp,
//                     isPending: false,
//                   ),
//                 );
//               }
//             });
//             _saveCache();
//             _scrollToBottom();
//             return;
//           }

//           final exists = _messages.any((m) => m.id == msgId);
//           if (!exists) {
//             setState(() {
//               _messages.add(
//                 _Message(
//                   id: msgId,
//                   sender: sender,
//                   text: text,
//                   timestamp: timestamp,
//                   isPending: false,
//                 ),
//               );
//             });
//             _saveCache();
//             _scrollToBottom();
//           }
//         }
//       },
//       onError: (e) {
//         debugPrint('WS error: $e');
//       },
//       onDone: () {
//         debugPrint('WS closed');
//       },
//     );
//   }

//   void _sendMessage() {
//     final text = _controller.text.trim();
//     if (text.isEmpty) return;

//     final msgId = _uuid.v4();

//     setState(() {
//       _messages.add(
//         _Message(
//           id: msgId,
//           sender: mySenderId,
//           text: text,
//           isPending: true,
//           timestamp: DateTime.now(),
//         ),
//       );
//       _controller.clear();
//     });
//     _saveCache();
//     _scrollToBottom();

//     _channel.sink.add(
//       jsonEncode({
//         'type': 'message',
//         'id': msgId,
//         'message': text,
//         'sender': mySenderId,
//       }),
//     );
//   }

//   @override
//   void dispose() {
//     _controller.dispose();
//     _scrollController.dispose();
//     try {
//       _channel.sink.close();
//     } catch (_) {}
//     super.dispose();
//   }

//   Widget _buildMessage(_Message msg) {
//     final isUser = msg.sender == mySenderId;
//     return Align(
//       alignment: isUser ? Alignment.centerRight : Alignment.centerLeft,
//       child: Container(
//         margin: const EdgeInsets.symmetric(vertical: 4, horizontal: 8),
//         padding: const EdgeInsets.all(12),
//         constraints: const BoxConstraints(maxWidth: 300),
//         decoration: BoxDecoration(
//           color: isUser ? const Color(0xFFFFD700) : Colors.grey.shade200,
//           borderRadius: BorderRadius.circular(12),
//         ),
//         child: Column(
//           crossAxisAlignment: CrossAxisAlignment.start,
//           children: [
//             Text(msg.text, style: const TextStyle(color: Colors.black)),
//             if (msg.timestamp != null)
//               Text(
//                 '${msg.timestamp!.hour.toString().padLeft(2, '0')}:${msg.timestamp!.minute.toString().padLeft(2, '0')}',
//                 style: const TextStyle(fontSize: 10, color: Colors.grey),
//               ),
//             if (msg.isPending)
//               const Padding(
//                 padding: EdgeInsets.only(top: 4),
//                 child: SizedBox(
//                   width: 12,
//                   height: 12,
//                   child: CircularProgressIndicator(strokeWidth: 2),
//                 ),
//               ),
//           ],
//         ),
//       ),
//     );
//   }

//   Future<void> _saveCache() async {
//     final prefs = await SharedPreferences.getInstance();
//     final key = '$_cacheKeyPrefix${widget.managerName}';
//     final list = _messages.map((m) => m.toJson()).toList();
//     await prefs.setString(key, jsonEncode(list));
//   }

//   Future<void> _loadCache() async {
//     final prefs = await SharedPreferences.getInstance();
//     final key = '$_cacheKeyPrefix${widget.managerName}';
//     final raw = prefs.getString(key);
//     if (raw == null) return;
//     try {
//       final List decoded = jsonDecode(raw);
//       setState(() {
//         _messages.clear();
//         for (final e in decoded) {
//           _messages.add(_Message.fromJson(Map<String, dynamic>.from(e)));
//         }
//       });
//     } catch (e) {
//       debugPrint('Failed to load chat cache: $e');
//     }
//   }

//   void _scrollToBottom() {
//     WidgetsBinding.instance.addPostFrameCallback((_) {
//       if (_scrollController.hasClients) {
//         _scrollController.animateTo(
//           _scrollController.position.maxScrollExtent + 80,
//           duration: const Duration(milliseconds: 250),
//           curve: Curves.easeOut,
//         );
//       }
//     });
//   }

//   @override
//   Widget build(BuildContext context) {
//     _messages.sort(
//       (a, b) => (a.timestamp ?? DateTime.now()).compareTo(
//         b.timestamp ?? DateTime.now(),
//       ),
//     );

//     return Scaffold(
//       appBar: AppBar(title: Text('Чат с менеджером: ${widget.managerName}')),
//       body: Column(
//         children: [
//           Expanded(
//             child: ListView.builder(
//               controller: _scrollController,
//               padding: const EdgeInsets.all(8),
//               itemCount: _messages.length,
//               itemBuilder: (_, index) => _buildMessage(_messages[index]),
//             ),
//           ),
//           Padding(
//             padding: const EdgeInsets.fromLTRB(8, 0, 8, 8),
//             child: Row(
//               children: [
//                 Expanded(
//                   child: TextField(
//                     controller: _controller,
//                     textInputAction: TextInputAction.send,
//                     onSubmitted: (_) => _sendMessage(),
//                     decoration: const InputDecoration(
//                       hintText: 'Введите сообщение...',
//                       border: OutlineInputBorder(),
//                       contentPadding: EdgeInsets.symmetric(horizontal: 12),
//                     ),
//                   ),
//                 ),
//                 const SizedBox(width: 8),
//                 ElevatedButton(
//                   onPressed: _sendMessage,
//                   child: const Icon(Icons.send),
//                 ),
//               ],
//             ),
//           ),
//         ],
//       ),
//     );
//   }
// }

// class _Message {
//   final String id;
//   final String sender;
//   final String text;
//   final bool isPending;
//   final DateTime? timestamp;

//   _Message({
//     required this.id,
//     required this.sender,
//     required this.text,
//     this.isPending = false,
//     this.timestamp,
//   });

//   _Message copyWith({
//     String? sender,
//     String? text,
//     bool? isPending,
//     DateTime? timestamp,
//   }) {
//     return _Message(
//       id: id,
//       sender: sender ?? this.sender,
//       text: text ?? this.text,
//       isPending: isPending ?? this.isPending,
//       timestamp: timestamp ?? this.timestamp,
//     );
//   }

//   Map<String, dynamic> toJson() => {
//     'id': id,
//     'sender': sender,
//     'text': text,
//     'isPending': isPending,
//     'timestamp': timestamp?.toIso8601String(),
//   };

//   factory _Message.fromJson(Map<String, dynamic> json) {
//     return _Message(
//       id: json['id'] as String,
//       sender: json['sender'] as String,
//       text: json['text'] as String,
//       isPending: json['isPending'] as bool? ?? false,
//       timestamp: json['timestamp'] != null
//           ? DateTime.tryParse(json['timestamp'])
//           : null,
//     );
//   }
// }

import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:uuid/uuid.dart';
import 'package:web_socket_channel/web_socket_channel.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:provider/provider.dart';
import 'package:brandbook_app/screens/cart/providers/auth_provider.dart';
import 'profile/login_screen.dart';

class ChatWithManagerScreen extends StatefulWidget {
  final String managerName;
  const ChatWithManagerScreen({super.key, required this.managerName});

  @override
  State<ChatWithManagerScreen> createState() => _ChatWithManagerScreen();
}

class _ChatWithManagerScreen extends State<ChatWithManagerScreen> {
  final List<_Message> _messages = [];
  final TextEditingController _controller = TextEditingController();
  final ScrollController _scrollController = ScrollController();
  WebSocketChannel? _channel;
  final _uuid = const Uuid();
  late String _mySenderId;
  String? _authToken;
  static const String _cacheKeyPrefix = 'chat_cache_';
  bool _isConnecting = false;

  // NEW: держим ссылку на провайдера и флаг подписки
  AuthProvider? _authProvider;
  bool _authListenerAttached = false;

  @override
  void initState() {
    super.initState();
    // Подписка на AuthProvider и инициализация делаются после первого фрейма,
    // чтобы Provider уже был доступен в дереве
    WidgetsBinding.instance.addPostFrameCallback((_) {
      try {
        _authProvider = Provider.of<AuthProvider>(context, listen: false);
        if (_authProvider != null && !_authListenerAttached) {
          _authProvider!.addListener(_handleAuthChanged);
          _authListenerAttached = true;
        }
      } catch (_) {
        _authProvider = null;
      }
      _initIdentityAndConnect();
    });
  }

  Future<void> _initIdentityAndConnect() async {
    final prefs = await SharedPreferences.getInstance();

    // Ensure a stable local user ID for unique chat room identification
    String? storedUserId = prefs.getString('user_id');
    if (storedUserId == null ||
        storedUserId.trim().isEmpty ||
        storedUserId.toLowerCase() == 'null') {
      storedUserId = _uuid.v4();
      await prefs.setString('user_id', storedUserId);
    }
    _mySenderId = storedUserId;

    // Try to get token/name from AuthProvider for authenticated chats
    try {
      final authProvider = Provider.of<AuthProvider>(context, listen: false);
      _authToken = authProvider.token;
      if (authProvider.isAuthenticated) {
        // Лучше использовать стабильный userId из провайдера, если есть
        _mySenderId = authProvider.name ?? authProvider.name ?? storedUserId;
      }
    } catch (_) {
      _authToken = null; // Fallback to unauthenticated mode
    }

    await _loadCache();
    _connect();
  }

  // NEW: обработчик изменений авторизации
  void _handleAuthChanged() async {
    try {
      final newToken = _authProvider?.token;
      final prevToken = _authToken;
      final prevSenderId = _mySenderId;

      _authToken = newToken;

      if (_authProvider != null && _authProvider!.isAuthenticated) {
        final newSender =
            _authProvider!.name ?? _authProvider!.name ?? _mySenderId;
        _mySenderId = newSender;
      } else {
        final prefs = await SharedPreferences.getInstance();
        final storedUserId = prefs.getString('user_id');
        _mySenderId = (storedUserId != null && storedUserId.isNotEmpty)
            ? storedUserId
            : _uuid.v4();
      }

      // Если изменился sender или token — перезапускаем подключение и кеш
      if (prevSenderId != _mySenderId || prevToken != _authToken) {
        try {
          await _channel?.sink.close();
        } catch (_) {}
        _channel = null;
        _isConnecting = false;

        await _loadCache();
        _connect();

        if (mounted) setState(() {});
      }
    } catch (e) {
      debugPrint('Auth change handler error: $e');
    }
  }

  void _connect() async {
    if (_channel != null || _isConnecting) return;

    _isConnecting = true;
    final role = Uri.encodeComponent(_mySenderId);
    // NEW: кодируем managerName и sender в имени комнаты
    final roomName = Uri.encodeComponent(
      'chat_${_mySenderId}_${widget.managerName}',
    );
    final tokenPart = (_authToken != null && _authToken!.isNotEmpty)
        ? '&token=${Uri.encodeComponent(_authToken!)}'
        : '';
    final uri = 'ws://127.0.0.1:8000/ws/chat/$roomName/?sender=$role$tokenPart';

    try {
      _channel = WebSocketChannel.connect(Uri.parse(uri));
      setState(() {}); // Update UI to reflect connection status
    } catch (e) {
      debugPrint('WebSocket connection error: $e');
      _channel = null;
      _isConnecting = false;
      _scheduleReconnect();
      return;
    }

    _channel!.stream.listen(
      (data) {
        try {
          final map = jsonDecode(data as String);
          final type = map['type'] ?? 'message';

          if (type == 'history') {
            final List msgs = map['messages'] ?? [];
            setState(() {
              _messages.clear();
              for (final m in msgs) {
                _messages.add(
                  _Message(
                    id: m['id'] ?? _uuid.v4(),
                    sender: m['sender'] ?? 'unknown',
                    text: m['message'] ?? '',
                    timestamp: DateTime.tryParse(m['timestamp'] ?? ''),
                    isPending: false,
                  ),
                );
              }
            });
            _saveCache();
            _scrollToBottom();
            return;
          }

          if (type == 'message') {
            final msgId = map['id'] ?? _uuid.v4();
            final sender = map['sender'] ?? 'unknown';
            final text = map['message'] ?? '';
            final timestamp =
                DateTime.tryParse(map['timestamp'] ?? '') ?? DateTime.now();

            if (sender == _mySenderId) {
              setState(() {
                final idx = _messages.indexWhere((m) => m.id == msgId);
                if (idx != -1) {
                  _messages[idx] = _messages[idx].copyWith(
                    isPending: false,
                    timestamp: timestamp,
                  );
                } else {
                  _messages.add(
                    _Message(
                      id: msgId,
                      sender: sender,
                      text: text,
                      timestamp: timestamp,
                      isPending: false,
                    ),
                  );
                }
              });
            } else {
              final exists = _messages.any((m) => m.id == msgId);
              if (!exists) {
                setState(() {
                  _messages.add(
                    _Message(
                      id: msgId,
                      sender: sender,
                      text: text,
                      timestamp: timestamp,
                      isPending: false,
                    ),
                  );
                });
              }
            }
            _saveCache();
            _scrollToBottom();
          }
        } catch (e) {
          debugPrint('WebSocket message handling error: $e');
        }
      },
      onError: (e) {
        debugPrint('WebSocket error: $e');
        _channel = null;
        _isConnecting = false;
        _scheduleReconnect();
      },
      onDone: () {
        debugPrint('WebSocket closed');
        _channel = null;
        _isConnecting = false;
        _scheduleReconnect();
      },
    );
    _isConnecting = false;
  }

  void _scheduleReconnect() {
    Future.delayed(const Duration(seconds: 5), () {
      if (mounted && _channel == null && !_isConnecting) {
        _connect();
      }
    });
  }

  void _sendMessage() {
    final text = _controller.text.trim();
    if (text.isEmpty || _channel == null) return;

    final msgId = _uuid.v4();
    setState(() {
      _messages.add(
        _Message(
          id: msgId,
          sender: _mySenderId,
          text: text,
          isPending: true,
          timestamp: DateTime.now(),
        ),
      );
      _controller.clear();
    });
    _saveCache();
    _scrollToBottom();

    final payload = {
      'type': 'message',
      'id': msgId,
      'message': text,
      'sender': _mySenderId,
    };

    try {
      _channel!.sink.add(jsonEncode(payload));
    } catch (e) {
      debugPrint('Failed to send message: $e');
      _scheduleReconnect();
    }
  }

  @override
  void dispose() {
    if (_authProvider != null && _authListenerAttached) {
      try {
        _authProvider!.removeListener(_handleAuthChanged);
      } catch (_) {}
      _authListenerAttached = false;
    }
    _controller.dispose();
    _scrollController.dispose();
    _channel?.sink.close();
    _channel = null;
    super.dispose();
  }

  Widget _buildMessage(_Message msg) {
    final isUser = msg.sender == _mySenderId;
    return Align(
      alignment: isUser ? Alignment.centerRight : Alignment.centerLeft,
      child: Container(
        margin: const EdgeInsets.symmetric(vertical: 4, horizontal: 8),
        padding: const EdgeInsets.all(12),
        constraints: const BoxConstraints(maxWidth: 300),
        decoration: BoxDecoration(
          color: isUser ? const Color(0xFFFFD700) : Colors.grey.shade200,
          borderRadius: BorderRadius.circular(12),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.1),
              blurRadius: 4,
              offset: const Offset(0, 2),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              msg.text,
              style: const TextStyle(color: Colors.black, fontSize: 16),
            ),
            if (msg.timestamp != null)
              Text(
                '${msg.timestamp!.hour.toString().padLeft(2, '0')}:${msg.timestamp!.minute.toString().padLeft(2, '0')}',
                style: const TextStyle(fontSize: 10, color: Colors.grey),
              ),
            if (msg.isPending)
              const Padding(
                padding: EdgeInsets.only(top: 4),
                child: SizedBox(
                  width: 12,
                  height: 12,
                  child: CircularProgressIndicator(strokeWidth: 2),
                ),
              ),
          ],
        ),
      ),
    );
  }

  Future<void> _saveCache() async {
    final prefs = await SharedPreferences.getInstance();
    // NEW: кодируем части ключа чтобы избежать проблем с спецсимволами
    final key =
        '$_cacheKeyPrefix${Uri.encodeComponent(widget.managerName)}_${Uri.encodeComponent(_mySenderId)}';
    final list = _messages.map((m) => m.toJson()).toList();
    try {
      await prefs.setString(key, jsonEncode(list));
    } catch (e) {
      debugPrint('Failed to save chat cache: $e');
    }
  }

  Future<void> _loadCache() async {
    final prefs = await SharedPreferences.getInstance();
    final key =
        '$_cacheKeyPrefix${Uri.encodeComponent(widget.managerName)}_${Uri.encodeComponent(_mySenderId)}';
    final raw = prefs.getString(key);
    if (raw == null) {
      setState(() => _messages.clear());
      return;
    }
    try {
      final List decoded = jsonDecode(raw);
      setState(() {
        _messages.clear();
        for (final e in decoded) {
          _messages.add(_Message.fromJson(Map<String, dynamic>.from(e)));
        }
      });
      _scrollToBottom();
    } catch (e) {
      debugPrint('Failed to load chat cache: $e');
    }
  }

  void _scrollToBottom() {
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (_scrollController.hasClients) {
        _scrollController.animateTo(
          _scrollController.position.maxScrollExtent + 80,
          duration: const Duration(milliseconds: 250),
          curve: Curves.easeOut,
        );
      }
    });
  }

  // @override
  // Widget build(BuildContext context) {
  //   _messages.sort(
  //     (a, b) => (a.timestamp ?? DateTime.now()).compareTo(
  //       b.timestamp ?? DateTime.now(),
  //     ),
  //   );

  //   return Scaffold(
  //     appBar: AppBar(
  //       title: Text('Чат с менеджером: ${widget.managerName}'),
  //       actions: [
  //         if (_channel == null && !_isConnecting)
  //           IconButton(
  //             icon: const Icon(Icons.refresh),
  //             onPressed: _connect,
  //             tooltip: 'Переподключиться',
  //           ),
  //       ],
  //     ),
  //     body: Column(
  //       children: [
  //         Expanded(
  //           child: ListView.builder(
  //             controller: _scrollController,
  //             padding: const EdgeInsets.all(8),
  //             itemCount: _messages.length,
  //             itemBuilder: (_, index) => _buildMessage(_messages[index]),
  //           ),
  //         ),
  //         Padding(
  //           padding: const EdgeInsets.fromLTRB(8, 0, 8, 8),
  //           child: Row(
  //             children: [
  //               Expanded(
  //                 child: TextField(
  //                   controller: _controller,
  //                   textInputAction: TextInputAction.send,
  //                   onSubmitted: (_) => _sendMessage(),
  //                   decoration: InputDecoration(
  //                     hintText: 'Введите сообщение...',
  //                     border: const OutlineInputBorder(),
  //                     contentPadding: const EdgeInsets.symmetric(
  //                       horizontal: 12,
  //                     ),
  //                     enabled: _channel != null,
  //                   ),
  //                 ),
  //               ),
  //               const SizedBox(width: 8),
  //               ElevatedButton(
  //                 onPressed: _channel != null ? _sendMessage : null,
  //                 child: const Icon(Icons.send),
  //               ),
  //             ],
  //           ),
  //         ),
  //       ],
  //     ),
  //   );
  // }
  @override
  Widget build(BuildContext context) {
    final auth = Provider.of<AuthProvider>(context);

    // Если пользователь не авторизован — показываем кнопку "Войти / Зарегистрироваться"
    if (!auth.isAuthenticated) {
      return Scaffold(
        appBar: AppBar(title: const Text('Чат с менеджером')),
        body: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const Padding(
                padding: EdgeInsets.symmetric(horizontal: 24.0),
                child: Text(
                  'Войдите или зарегистрируйтесь, чтобы начать чат с менеджером.',
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
                child: const Text('Войти / Зарегистрироваться'),
              ),
            ],
          ),
        ),
      );
    }

    // Пользователь авторизован — показываем обычный чат
    _messages.sort(
      (a, b) => (a.timestamp ?? DateTime.now()).compareTo(
        b.timestamp ?? DateTime.now(),
      ),
    );

    return Scaffold(
      appBar: AppBar(
        title: Text('Чат с менеджером: ${widget.managerName}'),
        actions: [
          if (_channel == null && !_isConnecting)
            IconButton(
              icon: const Icon(Icons.refresh),
              onPressed: _connect,
              tooltip: 'Переподключиться',
            ),
        ],
      ),
      body: Column(
        children: [
          Expanded(
            child: ListView.builder(
              controller: _scrollController,
              padding: const EdgeInsets.all(8),
              itemCount: _messages.length,
              itemBuilder: (_, index) => _buildMessage(_messages[index]),
            ),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(8, 0, 8, 8),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: _controller,
                    textInputAction: TextInputAction.send,
                    onSubmitted: (_) => _sendMessage(),
                    decoration: InputDecoration(
                      hintText: 'Введите сообщение...',
                      border: const OutlineInputBorder(),
                      contentPadding: const EdgeInsets.symmetric(
                        horizontal: 12,
                      ),
                      enabled: _channel != null,
                    ),
                  ),
                ),
                const SizedBox(width: 8),
                ElevatedButton(
                  onPressed: _channel != null ? _sendMessage : null,
                  child: const Icon(Icons.send),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _Message {
  final String id;
  final String sender;
  final String text;
  final bool isPending;
  final DateTime? timestamp;

  _Message({
    required this.id,
    required this.sender,
    required this.text,
    this.isPending = false,
    this.timestamp,
  });

  _Message copyWith({
    String? id,
    String? sender,
    String? text,
    bool? isPending,
    DateTime? timestamp,
  }) {
    return _Message(
      id: id ?? this.id,
      sender: sender ?? this.sender,
      text: text ?? this.text,
      isPending: isPending ?? this.isPending,
      timestamp: timestamp ?? this.timestamp,
    );
  }

  Map<String, dynamic> toJson() => {
    'id': id,
    'sender': sender,
    'message': text,
    'isPending': isPending,
    'timestamp': timestamp?.toIso8601String(),
  };

  factory _Message.fromJson(Map<String, dynamic> json) {
    return _Message(
      id: json['id'] as String? ?? const Uuid().v4(),
      sender: json['sender'] as String? ?? 'unknown',
      text: json['message'] as String? ?? json['text'] as String? ?? '',
      isPending: json['isPending'] as bool? ?? false,
      timestamp: json['timestamp'] != null
          ? DateTime.tryParse(json['timestamp'] as String)
          : null,
    );
  }
}
