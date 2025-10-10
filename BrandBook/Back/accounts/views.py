from rest_framework import generics, status
from rest_framework.response import Response
from rest_framework.renderers import JSONRenderer
from rest_framework.authtoken.models import Token
from .serializers import RegisterSerializer, LoginSerializer
from rest_framework.permissions import IsAuthenticated
from rest_framework.views import APIView
from .serializers import ProfileSerializer

class UTF8JSONRenderer(JSONRenderer):
    charset = 'utf-8'

class RegisterAPI(generics.CreateAPIView):
    serializer_class = RegisterSerializer
    renderer_classes = [UTF8JSONRenderer]

    def post(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        user = serializer.save()
        token = Token.objects.create(user=user)
        return Response({
            'token': token.key,
            'email': user.email,
            'name': user.name
        }, status=status.HTTP_201_CREATED)

class LoginAPI(generics.GenericAPIView):
    serializer_class = LoginSerializer
    renderer_classes = [UTF8JSONRenderer]  # Явно ставим рендерер с UTF-8

    def post(self, request, *args, **kwargs):
        serializer = self.get_serializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        user = serializer.validated_data['user']
        token, _ = Token.objects.get_or_create(user=user)
        return Response({
            'token': token.key,
            'email': user.email,
            'name': user.name
        })
class ProfileAPI(APIView):
    permission_classes = [IsAuthenticated]
    renderer_classes = [UTF8JSONRenderer]

    def get(self, request):
        serializer = ProfileSerializer(request.user)
        return Response(serializer.data)

    def patch(self, request):
        serializer = ProfileSerializer(request.user, data=request.data, partial=True)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)