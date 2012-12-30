from django.conf.urls import patterns, include, url

from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('server.views',
        url(r'^check_in$', 'check_in'),
        url(r'^$', 'index'),
        url(r'^listing$', 'listing'),
        url(r'^listing/(?P<clientId>\d+)/$', 'computer_profile'),
        url(r'^killswitch$', 'killswitch'),
        url(r'^logout/?$', 'logout_view'),

)

urlpatterns += patterns('',
    url(r'^admin/', include(admin.site.urls)),
)
