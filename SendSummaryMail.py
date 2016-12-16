import utils
import datetime


def main():
    mail=utils.mailing();
    mail.sendmail(mail.Summary())                
            

if __name__=='__main__':
    main()
            
