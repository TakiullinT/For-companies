import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import 'gallery.dart';
import 'contact_manager.dart';
import 'package:brandbook_app/screens/profile/profile_screen.dart';

class AboutScreen extends StatelessWidget {
  const AboutScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final List<String> imagePaths = List.generate(
      18,
      (index) => 'assets/images/${index + 1}.png',
    );

    return Scaffold(
      appBar: AppBar(
        title: const Text('О компании'),
        actions: [
          IconButton(
            icon: const Icon(Icons.person),
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => PersonalAccountScreen()),
              );
            },
          ),
        ],
      ),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: ListView(
          children: [
            sectionCard(
              icon: Icons.flag,
              title: 'Миссия и цель',
              content:
                  'Наша миссия — продвигать бизнес клиентов с помощью эффективных рекламных решений и стратегий.\n\n'
                  'Цель — стать лидером в маркетинговых и outdoor-услугах региона.',
            ),
            sectionCard(
              icon: Icons.settings,
              title: 'Принципы компании',
              content:
                  '• Качество: выполняем работу «качественно и вовремя»\n'
                  '• Надёжность: проверенные решения\n'
                  '• Ответственность: поддержка на каждом этапе\n'
                  '• Компетентность: квалифицированные специалисты\n'
                  '• Креатив: современные и эффективные идеи',
              buttonText: 'Скачать принципы',
              onButtonPressed: () async {
                final url = Uri.parse(
                  'https://docs.yandex.ru/docs/view?url=ya-disk-public%3A%2F%2FATuMmLj2omHI5MtokjynQzePnQlewUqRldqVOyPSTbYFaQdaUcUz2rzE9TzLoEvOq%2FJ6bpmRyOJonT3VoXnDag%3D%3D%3A%2FПринципы%20Р%20Брзндбук.docx&name=Принципы%20Р%20Брзндбук.docx&nosw=1',
                );
                if (await canLaunchUrl(url)) {
                  await launchUrl(url, mode: LaunchMode.externalApplication);
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Не удалось открыть ссылку')),
                  );
                }
              },
              buttonIcon: Icons.download,
            ),
            sectionCard(
              icon: Icons.history,
              title: 'История и достижения',
              content:
                  'Компания основана в 2010 году. Более 416 рекламных поверхностей в регионе.\n\n'
                  'Среди проектов — наружные кампании, брендинг, сайты, сувенирная продукция.\n'
                  'Награждена благодарственными письмами от партнёров и клиентов.',
            ),
            const SizedBox(height: 16),

            const Text(
              '📷 Галерея проектов',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 12),
            GridView.builder(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              itemCount: imagePaths.length,
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 3,
                crossAxisSpacing: 8,
                mainAxisSpacing: 8,
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
            const SizedBox(height: 24),

            ElevatedButton.icon(
              icon: const Icon(Icons.chat),
              label: const Text('Связаться с менеджером'),
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (_) => ContactManagerScreen()),
                );
              },
            ),
            const SizedBox(height: 32),

            sectionCard(
              icon: Icons.folder,
              title: 'Наши работы',
              content: 'Примеры реализованных проектов доступны для загрузки.',
              buttonText: 'Скачать файл',
              buttonIcon: Icons.download,
              onButtonPressed: () async {
                final url = Uri.parse(
                  'https://disk.yandex.ru/i/ApQ8uhi4shYymw',
                );
                if (await canLaunchUrl(url)) {
                  await launchUrl(url, mode: LaunchMode.externalApplication);
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Не удалось открыть ссылку')),
                  );
                }
              },
            ),
            sectionCard(
              icon: Icons.slideshow,
              title: 'Презентация компании',
              content: 'Краткий обзор услуг, кейсов и философии бренда.',
              buttonText: 'Скачать презентацию',
              buttonIcon: Icons.download,
              onButtonPressed: () async {
                final url = Uri.parse(
                  'https://docs.yandex.ru/docs/view?url=ya-disk-public%3A%2F%2FATuMmLj2omHI5MtokjynQzePnQlewUqRldqVOyPSTbYFaQdaUcUz2rzE9TzLoEvOq%2FJ6bpmRyOJonT3VoXnDag%3D%3D%3A%2FBRANDBOOK%20presentation%202.pdf&name=BRANDBOOK%20presentation%202.pdf&nosw=1',
                );
                if (await canLaunchUrl(url)) {
                  await launchUrl(url, mode: LaunchMode.externalApplication);
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Не удалось открыть ссылку')),
                  );
                }
              },
            ),
          ],
        ),
      ),
    );
  }

  Widget sectionCard({
    required IconData icon,
    required String title,
    required String content,
    String? buttonText,
    IconData? buttonIcon,
    VoidCallback? onButtonPressed,
  }) {
    return Card(
      elevation: 2,
      margin: const EdgeInsets.only(bottom: 20),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(icon, color: Colors.black, size: 28),
                const SizedBox(width: 8),
                Text(
                  title,
                  style: const TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 12),
            Text(content, style: const TextStyle(fontSize: 16)),
            if (buttonText != null && onButtonPressed != null) ...[
              const SizedBox(height: 16),
              ElevatedButton.icon(
                icon: Icon(buttonIcon ?? Icons.link),
                label: Text(buttonText),
                onPressed: onButtonPressed,
              ),
            ],
          ],
        ),
      ),
    );
  }
}
