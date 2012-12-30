from django.http import HttpResponse
from django.shortcuts import render, redirect

from server.models import Computer

'''
Each function is a "view".
It takes a request object, as well as any additional info from the url.
It returns an HttpResponse object.
'render' is a shortcut function that renders html from a template by passing in
variables, render returns an HttpResponse object.
render(request, template path from template_dir setting in settings.py,
        dictionary for mapping variables from code to template)
'''

def index(request):
    '''Main page html. Static content'''

    return render(request, 'server/index.html')


def listing(request):
    '''Main listing of all the computers'''

    params = {'computers': Computer.objects.all()}
    # computer.objects.all() returns a list of all of the computer objects in
    # the db
    return render(request, 'server/listing.html', params)


def computer_profile(request, clientId):
    '''The page for an individual computer.

    This is an HTML form for changing the status of a computer.
    clientId is an integer that is parsed by django from the url
    so if the url is /listing/2, clientId is 2'''

    computer = Computer.objects.get(pk=clientId) # DB query 
    if request.method == 'POST': # process form
        computer.status = request.POST['status']
        computer.save()
        return redirect('/listing')
    else: # GET request (Default) - return form
        params = {'computer': computer}
        return render(request, 'server/computer_profile.html', params)


def check_in(request):
    return HttpResponse('hey')
