function a = fun_y_acceleration(x, y, v, u)

g = 9.82;               % gravitation [m/s^2]
m = 9.0;                % weight [kg] 
k = 294.6;              % spring constant [N/m]
b = 10;                 % dampening [Ns/m]
angleR = 3*pi/8;        % kick angle right leg
angleL = 5*pi/8;        % kick angle left leg
bandStart = [0.0, 2.0]; % where the elastic band is fixed

dist = -norm([x,y]);
theta = atan2((bandStart(2)-y),(bandStart(1)-x));

u = u(1,1)*sin(angleR)*(1-(exp(min(y,0)))) + u(2,1)*sin(angleL)*(1-(exp(min(y,0))));
a = (1/m)*(u - k*sin(theta)*dist - b*v - m*g);
end

