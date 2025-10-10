import 'package:flutter/material.dart';

class GalleryScreen extends StatelessWidget {
  final List<String> imagePaths = List.generate(
    18,
    (index) => 'assets/images/${index + 1}.png',
  );

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Галерея')),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: GridView.builder(
          itemCount: imagePaths.length,
          gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
            crossAxisCount: 3,
            mainAxisSpacing: 8,
            crossAxisSpacing: 8,
            childAspectRatio: 1,
          ),
          itemBuilder: (context, index) {
            return GestureDetector(
              onTap: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => FullscreenImageScreen(
                      imagePaths: imagePaths,
                      initialIndex: index,
                    ),
                  ),
                );
              },
              child: ClipRRect(
                borderRadius: BorderRadius.circular(8),
                child: Image.asset(imagePaths[index], fit: BoxFit.cover),
              ),
            );
          },
        ),
      ),
    );
  }
}

class FullscreenImageScreen extends StatefulWidget {
  final List<String> imagePaths;
  final int initialIndex;

  const FullscreenImageScreen({
    super.key,
    required this.imagePaths,
    required this.initialIndex,
  });

  @override
  State<FullscreenImageScreen> createState() => _FullscreenImageScreenState();
}

class _FullscreenImageScreenState extends State<FullscreenImageScreen> {
  late PageController _pageController;
  late int _currentIndex;

  @override
  void initState() {
    super.initState();
    _currentIndex = widget.initialIndex;
    _pageController = PageController(initialPage: _currentIndex);
  }

  @override
  void dispose() {
    _pageController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.black,
      appBar: AppBar(
        backgroundColor: Colors.black,
        iconTheme: const IconThemeData(color: Colors.white),
      ),
      body: PageView.builder(
        controller: _pageController,
        itemCount: widget.imagePaths.length,
        onPageChanged: (index) {
          setState(() {
            _currentIndex = index;
          });
        },
        itemBuilder: (context, index) {
          return Center(
            child: InteractiveViewer(
              child: Image.asset(widget.imagePaths[index]),
            ),
          );
        },
      ),
    );
  }
}
