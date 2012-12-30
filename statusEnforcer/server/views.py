from django.http import HttpResponse
from django.shortcuts import render, redirect

from server.models import Computer

def index(request):
    return render(request, 'server/index.html')


def listing(request):
    params = {'computers': Computer.objects.all()}
    return render(request, 'server/listing.html', params)


def computer_profile(request, clientId):
    computer = Computer.objects.get(pk=clientId)
    if request.method == 'POST':
        computer.status = request.POST['status']
        computer.save()
        return redirect('/listing')
    params = {'computer': computer}
    return render(request, 'server/computer_profile.html', params)
