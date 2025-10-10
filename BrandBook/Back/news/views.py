from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
from .models import News
from .serializers import NewsSerializer

class NewsListView(APIView):
    def get(self, request):
        news = News.objects.all().order_by('-published_at')
        serializer = NewsSerializer(news, many=True)
        return Response(serializer.data)

    def post(self, request):
        serializer = NewsSerializer(data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data, status=status.HTTP_201_CREATED)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
