import smtplib, os
import datetime

class mailing:  

    #Define variables to hold email parameters
    SMTPServer='SRI-QA-FILESERVER'
    sender = 'tests@automatic.tl'
    receiver = 'tharindal@99x.lk'
        
    def sendmail(self, message): 
        try:
            smtpObj = smtplib.SMTP(self.SMTPServer)
            smtpObj.sendmail(self.sender, self.receiver, message)   
        except Exception as e:
             print("e-mail sending error occured:  " + e)
           

    def Summary(self):

        readlog=logging()
        Summarymsg = "From: Auto Tests <tests@automatic.tl>" + "\n" + \
                "To: Build downloader <tharindal@99x.lk>" + "\n" + \
                "Subject: Test Execution Summary - " + str(datetime.datetime.today()) +"\n\n" + \
                 ""+readlog.readSummary() +"\n" + readlog.readVersion()

        return (Summarymsg)
 

class logging:

    def  readSummary(self):
        basedir="C:\\GIT\\Telerik\\CS\\TestResults"
        if os.path.exists(basedir):
            log=open(basedir+"\\"+"Summary.log", "r")
            if os.stat(basedir+"\\"+"Summary.log").st_size!=0:
                logstr=log.read()
                return str(logstr)
                log.close()

        else:
         return "empty log file"
         log.close()

    def  readVersion(self):
        basedir="C:\\GIT\\Telerik\\CS\\TestResults"
        if os.path.exists(basedir):
            log=open(basedir+"\\"+"version.log", "r")
            if os.stat(basedir+"\\"+"version.log").st_size!=0:
                logstr=log.read()
                return str(logstr)
                log.close()

        else:
         return "empty log file"
         log.close()

    def  readGitLog(self):
        basedir="C:\\GIT\\Telerik\\CS\\SupportingScripts"
        if os.path.exists(basedir):
            log=open(basedir+"\\"+"pull.log", "r")
            if os.stat(basedir+"\\"+"pull.log").st_size!=0:
                logstr=log.read()
                return str(logstr)
                log.close()

        else:
         return "empty log file"
         log.close()
