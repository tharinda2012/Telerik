__author__ = 'tharindal'
import shutil
import smtplib
import datetime
import zipfile
import tempfile
from email import encoders
from email.mime.text import MIMEText
from email.mime.base import MIMEBase
from email.mime.multipart import MIMEMultipart
import utils

def send_file_zipped(recipients, sender='tests@automatic.tl'):

    summaryfolder='C:\\GIT\\Telerik\\CS\\TestResults'
    shutil.make_archive("summary","zip",summaryfolder)
    zf = tempfile.TemporaryFile(prefix='mail', suffix='.zip')
    zip = zipfile.ZipFile(zf, 'w')
    zip.write('summary.zip')
    zip.close()
    zf.seek(0)

    # Create the message
    readlog=utils.logging()
    themsg = MIMEMultipart()
    themsg['Subject'] = "Subject: Test Execution Summary - " + str(datetime.datetime.today())
    themsg['To'] = ', '.join(recipients)
    themsg['From'] = sender
    themsg.preamble = 'I am not using a MIME-aware mail reader.\n'
    msg = MIMEBase('application', 'zip')
    msg.set_payload(zf.read())
    encoders.encode_base64(msg)
    fullcontent=MIMEText(readlog.readSummary() + "\n\n" + readlog.readVersion())
    msg.add_header('Content-Disposition', 'attachment', filename="summary" + '.zip')

    themsg.attach(fullcontent)

    themsg.attach(msg)
    themsg = themsg.as_string()

    # send the message
    smtp = smtplib.SMTP('172.16.94.35')
    #smtp.connect()
    smtp.sendmail(sender, recipients, themsg)
    smtp.close()

def main():
    send_file_zipped(['tharindal@99x.lk'])

if __name__ == '__main__':
    main()