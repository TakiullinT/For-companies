from django.contrib import admin
from django.contrib.auth.admin import UserAdmin as BaseUserAdmin
from .models import User

class UserAdmin(BaseUserAdmin):
    list_display = ('email', 'name', 'is_staff', 'is_superuser')
    search_fields = ('email', 'name')
    ordering = ('email',)
    fieldsets = (
        (None, {'fields': ('email', 'password')}),
        ('Персональная информация', {'fields': ('name',)}),
        ('Права', {'fields': ('is_staff', 'is_superuser', 'groups', 'user_permissions')}),
    )
    add_fieldsets = (
        (None, {
            'classes': ('wide',),
            'fields': ('email', 'name', 'password1', 'password2'),
        }),
    )

admin.site.register(User, UserAdmin)
