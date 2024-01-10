MODULE Module1
        CONST robtarget Target_40:=[[202.164306641,50,100],[0,0,1,0],[0,0,0,0],[135,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget Target_50:=[[150,-0.725036621,100],[0,-0.707106781,0.707106781,0],[0,0,0,0],[135,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget Target_60:=[[194.348602295,-50,100],[0.707106781,-0.707106781,0,0],[-2,-4,0,4],[135,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget Target_70:=[[250,-7.987426758,100],[0,0.707106781,0.707106781,0],[0,0,0,0],[135,9E+09,9E+09,9E+09,9E+09,9E+09]];
!***********************************************************
    !
    ! Module:  Module1
    !
    ! Description:
    !   <Insert description here>
    !
    ! Author: Balint
    !
    ! Version: 1.0
    !
    !***********************************************************
    
    
    !***********************************************************
    !
    ! Procedure main
    !
    !   This is the entry point of your program
    !
    !***********************************************************
    PROC main()
        Path_10;
    ENDPROC
    PROC Path_10()
        MoveJ Target_40,v1000,z100,tool0\WObj:=wobj0;
        MoveJ Target_50,v1000,z100,tool0\WObj:=wobj0;
        MoveJ Target_60,v1000,z100,tool0\WObj:=wobj0;
        MoveJ Target_70,v1000,z100,tool0\WObj:=wobj0;
    ENDPROC
ENDMODULE