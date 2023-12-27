using System;

namespace VirtualGuard.Stubs
{
    public class Limiter
    {
        public static void Limit()
        {
            Console.Title = "virtualguard.io";

            string s = @"
  .  . .  .  . .  .  . .  .  . .  .  . .  .  . .  .  . .  . 
   .       .       .       . ::    .       .       .        
     .  .    .  .    .  .  t.SS88     . .    .  .    .  . . 
 .       .       .    .;@8 88888  88%     .      .          
   .  .   8     8     8S888S888888888SS  8 8   % 8.   . .  .
  .    . tSX888888888888X8888888S888XX8888888@888Xt .       
    .    SSXX888888X888X88888%;:88888S8 8S8X8 8888%..    .  
  .   .  S888S@88 88888. ;X%:  .%   :8@8888888XX@8t  . .   .
    .    tS88@88XtXXt::      .       ..%X@8St88XXSS         
  .      X8@@88     . .     .  .         .  ;888S8 .. .  .  
     .  .tX888SX.         .         .  .   .;8888 @ .
   .  .  . 8 88:..   .      .    .   .   . ..@8 8 :         
        ..888 88  .     .      .   .       S8:@8 ;  . . . . 
 .  .    .;@88:S t .  .   .  .        . . : .X:@ ; .        
   .  .  ..@888:8S% .           . .      tS;X888;     .   . 
            888@ 8X;  . .  .  .     .  .;S88%8.    .   .    
 .  .  . . : 8:@8;888;      .    .    :88:8@88@  .   .   .  
   .        .  888%8SX;S..     .   .S;8t88%88t     .    .   
     .  .       ;8888%@8S; .      t:8@88888 :  . .    .     
 .    .   .    .:8S8888888 @.. .@ @88%@88@8S.       .    .  
   .        .   .: %88888tX8@:;88@%888888:: . .  .    .   . 
  .  .  .    .  .  ;t.88%888SX888888X8%:.   ..     .    .   
          .      .   .:.@@88888888 8t:  ..  .  .  .  .      
  . . .    .  .            ;@X8@8S .   .  .  .   .     . .  
        .      . . . . .  ..  %;;    .  .     .     .     . 
  .  .    .  .                  . .      . .    .  .  .     
       .        .  .  . . .    . .  .  .     .          .   ";
            Console.WriteLine(s);
            Console.WriteLine(
                "This application is protected by a trial version of VirtualGuard. You can purchase a full license @ https://virtualguard.io/.");
            Console.WriteLine("Press any key to begin execution of the program.");
            Console.ReadKey();
        }
    }
}