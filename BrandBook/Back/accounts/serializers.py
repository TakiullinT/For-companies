from rest_framework import serializers
from django.contrib.auth import authenticate
from .models import User  # Убедись, что это кастомная модель

class RegisterSerializer(serializers.ModelSerializer):
    password = serializers.CharField(write_only=True)

    class Meta:
        model = User
        fields = ['email', 'name', 'password']

    def create(self, validated_data):
        return User.objects.create_user(**validated_data)

class LoginSerializer(serializers.Serializer):
    email = serializers.EmailField()
    password = serializers.CharField(write_only=True)

    def validate(self, attrs):
        user = authenticate(email=attrs['email'], password=attrs['password'])
        if not user:
            raise serializers.ValidationError('Неверный логин или пароль')
        attrs['user'] = user
        return attrs

# ✅ Добавляем ProfileSerializer
class ProfileSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ['email', 'name']  # email можно оставить, но обновление зависит от требований
