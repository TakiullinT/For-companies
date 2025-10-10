import 'package:brandbook_app/screens/cart/cart_screen.dart';
import 'package:brandbook_app/screens/tools_screens/brending_screen.dart';
import 'package:brandbook_app/screens/tools_screens/laser_cutting_screen.dart';
import 'package:brandbook_app/screens/tools_screens/signboards_screen.dart';
import 'package:brandbook_app/screens/tools_screens/transport_screen.dart';
import 'package:flutter/material.dart';
import 'package:brandbook_app/components/tool_card.dart';
import 'outdoor_calculator_screen.dart';
import 'radio_media_planning_screen.dart';
import 'print_estimation_screen.dart';
import 'interior_print_screen.dart';
import 'signs_map_screen.dart';
import 'souvenir_screen.dart';
import 'brief_screen.dart';
import 'marketing_strategy_screen.dart';
import 'package:provider/provider.dart';
import 'package:brandbook_app/screens/cart/cart_provider.dart';

class ToolsScreen extends StatelessWidget {
  const ToolsScreen({super.key});

  static const List<String> _toolTitles = [
    'Наружная реклама',
    'Радио реклама',
    'Полиграфия',
    'Интерьерная печать',
    'Указатели и навигация',
    'Сувенирная продукция',
    'Бриф на проект',
    'Маркетинговая стратегия',
    'Брендинг',
    'Брендирование транспорта',
    'Лазерная резка',
    'Вывески',
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Услуги', selectionColor: Color(0xFFFFD700)),
        actions: [
          IconButton(
            icon: const Icon(Icons.search),
            onPressed: () {
              showSearch(
                context: context,
                delegate: ToolSearchDelegate(_toolTitles, context),
              );
            },
          ),
          Consumer<CartProvider>(
            builder: (context, cart, child) => IconButton(
              icon: Stack(
                children: [
                  const Icon(Icons.shopping_cart),
                  if (cart.itemCount > 0)
                    Positioned(
                      right: 0,
                      top: 0,
                      child: Container(
                        padding: const EdgeInsets.all(2),
                        decoration: const BoxDecoration(
                          color: Colors.red,
                          shape: BoxShape.circle,
                        ),
                        constraints: const BoxConstraints(
                          minWidth: 16,
                          minHeight: 16,
                        ),
                        child: Text(
                          '${cart.itemCount}',
                          style: const TextStyle(
                            color: Colors.white,
                            fontSize: 10,
                          ),
                          textAlign: TextAlign.center,
                        ),
                      ),
                    ),
                ],
              ),
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => const CartScreen()),
                );
              },
            ),
          ),
        ],
      ),

      body: SingleChildScrollView(
        child: Column(
          children: [
            _buildSectionHeader('Калькуляторы стоимости'),
            ToolCard(
              title: 'Наружная реклама',
              icon: Icons.outdoor_grill,
              color: Colors.blue,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (_) => const OutdoorCalculatorScreen(),
                ),
              ),
            ),
            ToolCard(
              title: 'Радио реклама',
              icon: Icons.radio,
              color: Colors.green,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (_) => const RadioMediaPlanningScreen(),
                ),
              ),
            ),
            ToolCard(
              title: 'Полиграфия',
              icon: Icons.print,
              color: Colors.orange,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (_) => const PrintEstimationScreen(),
                ),
              ),
            ),
            ToolCard(
              title: 'Интерьерная печать',
              icon: Icons.wallpaper,
              color: Colors.purple,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const InteriorPrintScreen()),
              ),
            ),
            ToolCard(
              title: 'Указатели и навигация',
              icon: Icons.signpost,
              color: Colors.red,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const SignsMapScreen()),
              ),
            ),
            ToolCard(
              title: 'Сувенирная продукция',
              icon: Icons.card_giftcard,
              color: Colors.teal,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const SouvenirScreen()),
              ),
            ),
            ToolCard(
              title: 'Брендинг',
              icon: Icons.auto_awesome,
              color: Colors.pink,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const BrandingScreen()),
              ),
            ),
            ToolCard(
              title: 'Брендирование транспорта',
              icon: Icons.directions_car,
              color: Colors.cyan,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (_) => const TransportBrandingScreen(),
                ),
              ),
            ),
            ToolCard(
              title: 'Лазерная резка',
              icon: Icons.cut,
              color: Colors.deepOrange,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const LaserCuttingScreen()),
              ),
            ),
            ToolCard(
              title: 'Вывески',
              icon: Icons.light,
              color: Colors.brown,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const SignboardsScreen()),
              ),
            ),
            _buildSectionHeader('Планирование'),
            ToolCard(
              title: 'Бриф на проект',
              icon: Icons.description,
              color: Colors.indigo,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const BriefScreen()),
              ),
            ),
            ToolCard(
              title: 'Маркетинговая стратегия',
              icon: Icons.insights,
              color: Colors.amber,
              onTap: () => Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (_) => const MarketingStrategyScreen(),
                ),
              ),
            ),
            const SizedBox(height: 20),
          ],
        ),
      ),
    );
  }

  Widget _buildSectionHeader(String title) {
    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 24, 16, 8),
      child: Align(
        alignment: Alignment.centerLeft,
        child: Text(
          title,
          style: const TextStyle(
            fontSize: 18,
            fontWeight: FontWeight.w600,
            color: Colors.black,
          ),
        ),
      ),
    );
  }
}

class ToolSearchDelegate extends SearchDelegate<String> {
  final List<String> tools;
  final BuildContext parentContext;

  ToolSearchDelegate(this.tools, this.parentContext);

  @override
  List<Widget> buildActions(BuildContext context) => [
    IconButton(icon: const Icon(Icons.clear), onPressed: () => query = ''),
  ];

  @override
  Widget buildLeading(BuildContext context) => IconButton(
    icon: const Icon(Icons.arrow_back),
    onPressed: () => close(context, ''),
  );

  @override
  Widget buildResults(BuildContext context) {
    final results = tools
        .where((tool) => tool.toLowerCase().contains(query.toLowerCase()))
        .toList();
    return _buildResultsList(results);
  }

  @override
  Widget buildSuggestions(BuildContext context) {
    final suggestions = tools
        .where((tool) => tool.toLowerCase().contains(query.toLowerCase()))
        .toList();
    return _buildResultsList(suggestions);
  }

  Widget _buildResultsList(List<String> results) {
    return ListView.builder(
      itemCount: results.length,
      itemBuilder: (context, index) {
        final tool = results[index];
        return ListTile(
          title: Text(tool),
          onTap: () {
            close(context, tool);
            _navigateToTool(tool);
          },
        );
      },
    );
  }

  void _navigateToTool(String tool) {
    Widget? screen;

    switch (tool) {
      case 'Наружная реклама':
        screen = const OutdoorCalculatorScreen();
        break;
      case 'Радио реклама':
        screen = const RadioMediaPlanningScreen();
        break;
      case 'Полиграфия':
        screen = const PrintEstimationScreen();
        break;
      case 'Интерьерная печать':
        screen = const InteriorPrintScreen();
        break;
      case 'Указатели и навигация':
        screen = const SignsMapScreen();
        break;
      case 'Сувенирная продукция':
        screen = const SouvenirScreen();
        break;
      case 'Бриф на проект':
        screen = const BriefScreen();
        break;
      case 'Маркетинговая стратегия':
        screen = const MarketingStrategyScreen();
        break;
      case 'Брендинг':
        screen = const BrandingScreen();
        break;
      case 'Брендирование транспорта':
        screen = const TransportBrandingScreen();
        break;
      case 'Лазерная резка':
        screen = const LaserCuttingScreen();
        break;
      case 'Вывески':
        screen = const SignboardsScreen();
        break;
    }

    if (screen != null) {
      Navigator.push(parentContext, MaterialPageRoute(builder: (_) => screen!));
    }
  }
}
