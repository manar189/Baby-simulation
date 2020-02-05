function a = fun_x_acceleration(x, y, v, u)

g = 9.82;               % gravitation [m/s^2]
m = 9.0;                % weight [kg] 
k = 294.6;              % spring constant [N/m]
b = 10;                 % dampening [Ns/m]
angleR = 3*pi/8;        % kick angle right leg
angleL = 5*pi/8;        % kick angle left leg
bandStart = [0.0, 2.0]; % where the elastic band is fixed

dist = -norm([x,y]);
theta = atan2((bandStart(2)-y),(bandStart(1)-x));

ux = u(1,1)*cos(angleR)*(1-(exp(min(y,0)))) + u(2,1)*cos(angleL)*(1-(exp(min(y,0))));
a = (1/m)*(ux - k*cos(theta)*dist - b*v);
end

