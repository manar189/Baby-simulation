function a = fun_baby(y, v, u)

g = 9.82;   % gravitation [m/s^2]
m = 9.0;    % weight [kg] 
k = 294.6;  % spring constant [N/m]
b = 10;     % dampening [Ns/m]

u = u*(1-(exp(min(y,0))));  % dynamic force depending on position
a = (1/m)*(u - k*y - b*v - m*g); % acceleration
end

