import 'package:flutter/material.dart';
import 'news_screen.dart';
import 'tools_screens/tools_screen.dart';
import 'about_screen.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _currentIndex = 0;

  final List<Widget> _screens = [
    const NewsScreen(),
    const ToolsScreen(),
    const AboutScreen(),
  ];

  final List<BottomNavigationBarItem> _navItems = const [
    BottomNavigationBarItem(
      icon: Icon(Icons.article_outlined),
      activeIcon: Icon(Icons.article, size: 28),
      label: 'Новости',
    ),
    BottomNavigationBarItem(
      icon: Icon(Icons.design_services_outlined),
      activeIcon: Icon(Icons.design_services, size: 28),
      label: 'Услуги',
    ),
    BottomNavigationBarItem(
      icon: Icon(Icons.info_outline),
      activeIcon: Icon(Icons.info, size: 28),
      label: 'О нас',
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey[100],
      body: AnimatedSwitcher(
        duration: const Duration(milliseconds: 300),
        child: _screens[_currentIndex],
      ),
      bottomNavigationBar: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          border: const Border(top: BorderSide(color: Colors.grey, width: 0.2)),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.05),
              blurRadius: 10,
              offset: const Offset(0, -1),
            ),
          ],
        ),
        child: ClipRRect(
          borderRadius: const BorderRadius.only(
            topLeft: Radius.circular(16),
            topRight: Radius.circular(16),
          ),
          child: BottomNavigationBar(
            currentIndex: _currentIndex,
            onTap: (index) => setState(() => _currentIndex = index),
            selectedItemColor: Colors.black,
            unselectedItemColor: Colors.grey[600],
            selectedFontSize: 13,
            unselectedFontSize: 12,
            backgroundColor: Colors.white,
            type: BottomNavigationBarType.fixed,
            elevation: 0,
            items: _navItems,
          ),
        ),
      ),
    );
  }
}
