function [ax, ay] = baby_acceleration(x, y, vx, vy, u)

g = 9.82;                   % gravitation [m/s^2]
m = 9.0;                    % weight [kg] 
k = 145.5;                  % spring constant [N/m]
b = 8.0;                    % dampening [Ns/m]
kfc = 8;                   % kick force constant
thetaR = 3*pi/8;            % kick angle right leg
thetaL = 5*pi/8;            % kick angle left leg

bandStartR = [-0.2, 2.0]; % where the elastic band is fixed
bandEndR = [-0.1, 0.0];   % where band stops without weight
bandStartL = [0.2, 2.0];  % where the elastic band is fixed
bandEndL = [0.1, 0.0];    % where band stops without weight

bandLengthR = pdist([bandStartR; bandEndR], 'euclidean');
bandLengthL = pdist([bandStartL; bandEndL], 'euclidean');
bandStretchR = pdist([bandStartR; x,y], 'euclidean');
bandStretchL = pdist([bandStartL; x,y], 'euclidean');

stretchDistR = bandLengthR - bandStretchR;
stretchDistL = bandLengthL - bandStretchL;

% compression is opposite of stretch in the elastic bands
compRange = 0.5;     % how long compression increases acceleration
compLimit = 0.1;     % compression limit
if(stretchDistR > 0)
    stretchDistR = min(compLimit*exp(stretchDistR-compRange), ...
        compLimit);
end
if(stretchDistL > 0)
    stretchDistL = min(compLimit*exp(stretchDistL-compRange), ...
        compLimit);
end

phiR = atan2((bandStartR(2)-(y-bandEndR(2))), ...
    (bandStartR(1)-(x-bandEndR(1))));
phiL = atan2((bandStartL(2)-(y-bandEndL(2))), ...
    (bandStartL(1)-(x-bandEndL(1))));

ux = u(1,1)*cos(thetaR)*kfc*(1-(exp(min(y,0)))) ...
    + u(2,1)*cos(thetaL)*kfc*(1-(exp(min(y,0))));

uy = u(1,1)*sin(thetaR)*kfc*(1-(exp(min(y,0)))) ...
    + u(2,1)*sin(thetaL)*kfc*(1-(exp(min(y,0))));

ax = (1/m)*(ux - k*cos(phiR)*stretchDistR ...
    - k*cos(phiL)*stretchDistL - b*vx);

ay = (1/m)*(uy - k*sin(phiR)*stretchDistR ...
    - k*sin(phiL)*stretchDistL - b*vy - m*g);
end