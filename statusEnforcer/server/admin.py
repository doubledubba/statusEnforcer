from django.contrib import admin
from server.models import Computer


class ComputerAdmin(admin.ModelAdmin):
    #fields = ['name']
    list_display = ('name', 'pk')


admin.site.register(Computer, ComputerAdmin)
