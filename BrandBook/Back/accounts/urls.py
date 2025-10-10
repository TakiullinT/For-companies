from django.urls import path
from .views import RegisterAPI, LoginAPI, ProfileAPI

urlpatterns = [
    path('register/', RegisterAPI.as_view()),
    path('login/', LoginAPI.as_view()),
    path('profile/', ProfileAPI.as_view()),         # GET
    path('profile/update/', ProfileAPI.as_view()),  # PATCH
]