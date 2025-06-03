@17

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@17

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] == sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_0
D;JEQ
@SP
A=M
M=0
@JUMP_END_0
0;JMP
(JUMP_SUC_0)
@SP
A=M
M=-1
(JUMP_END_0)
// incremets sp
@SP
M=M+1

@17

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@16

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] == sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_1
D;JEQ
@SP
A=M
M=0
@JUMP_END_1
0;JMP
(JUMP_SUC_1)
@SP
A=M
M=-1
(JUMP_END_1)
// incremets sp
@SP
M=M+1

@16

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@17

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] == sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_2
D;JEQ
@SP
A=M
M=0
@JUMP_END_2
0;JMP
(JUMP_SUC_2)
@SP
A=M
M=-1
(JUMP_END_2)
// incremets sp
@SP
M=M+1

@892

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@891

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] < sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_3
D;JLT
@SP
A=M
M=0
@JUMP_END_3
0;JMP
(JUMP_SUC_3)
@SP
A=M
M=-1
(JUMP_END_3)
// incremets sp
@SP
M=M+1

@891

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@892

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] < sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_4
D;JLT
@SP
A=M
M=0
@JUMP_END_4
0;JMP
(JUMP_SUC_4)
@SP
A=M
M=-1
(JUMP_END_4)
// incremets sp
@SP
M=M+1

@891

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@891

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] < sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_5
D;JLT
@SP
A=M
M=0
@JUMP_END_5
0;JMP
(JUMP_SUC_5)
@SP
A=M
M=-1
(JUMP_END_5)
// incremets sp
@SP
M=M+1

@32767

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@32766

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] > sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_6
D;JGT
@SP
A=M
M=0
@JUMP_END_6
0;JMP
(JUMP_SUC_6)
@SP
A=M
M=-1
(JUMP_END_6)
// incremets sp
@SP
M=M+1

@32766

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@32767

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] > sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_7
D;JGT
@SP
A=M
M=0
@JUMP_END_7
0;JMP
(JUMP_SUC_7)
@SP
A=M
M=-1
(JUMP_END_7)
// incremets sp
@SP
M=M+1

@32766

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@32766

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
//sp[n-2] > sp[n-1] -1 true 0 false
D=M-D
@JUMP_SUC_8
D;JGT
@SP
A=M
M=0
@JUMP_END_8
0;JMP
(JUMP_SUC_8)
@SP
A=M
M=-1
(JUMP_END_8)
// incremets sp
@SP
M=M+1

@57

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@31

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@53

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
// adds sp[n-2] and sp[n-1]
M=D+M
// incremets sp
@SP
M=M+1

@112

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
// subtracts sp[n-2] and sp[n-1]
M=M-D
// incremets sp
@SP
M=M+1

// translate neg
@SP
M=M-1
A=M
M=-M
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
// performs & between sp[n-2] and sp[n-1]
M=D&M
// incremets sp
@SP
M=M+1

@82

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
//adds top stack number to D and goes to the adress of the second
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M    
       
// performs | between sp[n-2] and sp[n-1]
M=D|M
// incremets sp
@SP
M=M+1

// nots sp[n-1]
@SP
M=M-1
A=M
M=!M
@SP
M=M+1 
