from django.http import HttpResponse
from django.shortcuts import render, redirect, get_object_or_404
from django.views.decorators.csrf import csrf_exempt
from django.contrib.auth.decorators import login_required
from django.contrib.auth import logout

from statusEnforcer.settings import SECRET_KEY

import json
from server.models import Computer
from pytz import utc
from datetime import datetime

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

@login_required
def listing(request):
    '''Main listing of all the computers'''

    params = {'computers': Computer.objects.all()}
    # computer.objects.all() returns a list of all of the computer objects in
    # the db
    return render(request, 'server/listing.html', params)

@login_required
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


@login_required
def killswitch(request):
    if request.method == 'GET':
        params = {'computer': 'Apply global action',
                'gif': True}
        return render(request, 'server/computer_profile.html', params)
    elif request.method == 'POST':
        for computer in Computer.objects.all():
            status = request.POST['status']
            computer.status = status
            computer.save()
            print 'Setting %s to %s' % (computer, status)
        return redirect('/listing')
    
@csrf_exempt
def check_in(request):
    if not request.method == 'POST':
        return HttpResponse('away, hacker!')
    clientId = request.POST.get('clientId')
	
    if not clientId:
        name = request.POST.get('name')
        if not name:
            return HttpResponse('0')
        computer = Computer()
        computer.name = name
        computer.lastConnection = datetime.now(utc)
        computer.key = request.POST.get('key')
        computer.save()
        return HttpResponse(computer.pk, mimetype='text/plain')
		
    clientPws = request.POST.get('key')
    
    computer = get_object_or_404(Computer, pk=clientId)
    #authenticate the client
    if clientPws != computer.key:
        return HttpResponse('nope', mimetype='text/plain')
	
    computer.lastConnection = datetime.now(utc)
    computer.connected = True
    status = computer.status
    if computer.status != 'ok':
        computer.status = 'ok'
    computer.save()
    return HttpResponse(status + "~" + SECRET_KEY, mimetype='text/plain')


def logout_view(request):
    logout(request)
    return redirect('/')
