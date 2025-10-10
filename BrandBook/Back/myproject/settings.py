from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent

SECRET_KEY = 'w9m1&x%v(8q@!z^n2*o#u$fjr!4k=6b1r$2v%cm+_s7@#t0x8q'

DEBUG = True
ALLOWED_HOSTS = []

# === Приложения ===
INSTALLED_APPS = [
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',

    # API
    'rest_framework',
    'rest_framework.authtoken',

    # WebSocket
    'channels',

    # Custom apps
    'accounts',
    'order',
    'chat',
    'news'
]

# === Middleware ===
MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
]

# === URLs, Templates ===
ROOT_URLCONF = 'myproject.urls'

TEMPLATES = [
    {
        'BACKEND': 'django.template.backends.django.DjangoTemplates',
        'DIRS': [],
        'APP_DIRS': True,
        'OPTIONS': {
            'context_processors': [
                'django.template.context_processors.debug',
                'django.template.context_processors.request',
                'django.contrib.auth.context_processors.auth',
                'django.contrib.messages.context_processors.messages',
            ],
        },
    },
]

# === WSGI & ASGI ===
WSGI_APPLICATION = 'myproject.wsgi.application'
ASGI_APPLICATION = 'myproject.asgi.application'

# === База данных ===
DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.sqlite3',
        'NAME': BASE_DIR / 'db.sqlite3',
    }
}

# === Пользовательская модель ===
AUTH_USER_MODEL = 'accounts.User'

# === Django REST Framework ===
REST_FRAMEWORK = {
    'DEFAULT_AUTHENTICATION_CLASSES': [
        'rest_framework.authentication.TokenAuthentication',
    ],
}

DEFAULT_CHARSET = 'utf-8'

# === Redis и Channels ===
CHANNEL_LAYERS = {
    'default': {
        'BACKEND': 'channels_redis.core.RedisChannelLayer',
        'CONFIG': {
            'hosts': [('127.0.0.1', 6379)],
        },
    },
}

# === Локализация и время ===
LANGUAGE_CODE = 'ru-ru'
TIME_ZONE = 'Europe/Moscow'
USE_I18N = True
USE_TZ = True

# === Статика ===
STATIC_URL = '/static/'

# === Автополе ID по умолчанию ===
DEFAULT_AUTO_FIELD = 'django.db.models.BigAutoField'
