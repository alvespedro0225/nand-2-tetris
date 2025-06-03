@7

// pushes to sp
D=A
@SP
A=M
M=D
@SP
M=M+1
@8

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

