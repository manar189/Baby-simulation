function [ax, ay, alpha] = baby_rot_acceleration(x, y, vx, vy, angle, w, u)

m = 9.0;                    % baby weight [kg]
l = 0.75;                   % baby length [m]
r = 0.15;                   % baby radius [m]
g = 9.82;                   % gravitation [m/s^2] 
k = 145.5;                  % spring constant [N/m]
b = 8.0;                    % dampening [Ns/m]
bw = 0.8;                   % dampening for rotation [Nms^2/rad]
kfc = 15;                   % kick force constant
thetaR = 3*pi/8;            % kick angle right leg
thetaL = 5*pi/8;            % kick angle left leg

I = 1/4*m*r^2 + 1/12*m*l^2; % moment of inertia, solid cylinder
legPosR = [x-r,y-(l/2)];    % position of right leg
legPosL = [x+r,y-(l/2)];    % position of left leg

bandStartR = [-0.3, 2.0];   % where the elastic band is fixed
bandEndR = [-r, 0.0];       % where band stops without weight
bandStartL = [0.3, 2.0];    % where the elastic band is fixed
bandEndL = [r, 0.0];        % where band stops without weight

R = [cos(angle), -sin(angle); sin(angle), cos(angle)];

newEndR = (R*[bandEndR(1); bandEndR(2)])' + [x,y];
newEndL = (R*[bandEndL(1); bandEndL(2)])' + [x,y];

bandLengthR = pdist([bandStartR; bandEndR], 'euclidean');
bandLengthL = pdist([bandStartL; bandEndL], 'euclidean');
bandStretchR = pdist([bandStartR; newEndR], 'euclidean');
bandStretchL = pdist([bandStartL; newEndL], 'euclidean');

stretchDistR = bandLengthR - bandStretchR;
stretchDistL = bandLengthL - bandStretchL;

if(stretchDistR > 0)
    stretchDistR = 0;
end
if(stretchDistL > 0)
    stretchDistL = 0;
end

phiR = atan2((bandStartR(2)-newEndR(2)), ... 
    (bandStartR(1)-newEndR(1)));
phiL = atan2((bandStartL(2)-newEndL(2)), ...
    (bandStartL(1)-newEndL(1)));

bandForceR = k*stretchDistR;
bandForceL = k*stretchDistL;

ux = u(1,1)*cos(thetaR)*kfc*(1-(exp(min(y,0)))) ...
    + u(2,1)*cos(thetaL)*kfc*(1-(exp(min(y,0))));

uy = u(1,1)*sin(thetaR)*kfc*(1-(exp(min(y,0)))) ...
    + u(2,1)*sin(thetaL)*kfc*(1-(exp(min(y,0))));

ax = (1/m)*(ux - cos(phiR)*bandForceR ...
    - cos(phiL)*bandForceL - b*vx);

ay = (1/m)*(uy - sin(phiR)*bandForceR ...
    - sin(phiL)*bandForceL - b*vy - m*g);

torqueKickR = getTorque(u(1,1), legPosR, thetaR, [x,y]);
torqueKickL = getTorque(u(2,1), legPosL, thetaL, [x,y]);
torqueBandR = getTorque(bandForceR, newEndR, phiR, [x,y]);
torqueBandL = getTorque(bandForceL, newEndL, phiL, [x,y]);

alpha = -((torqueKickR + torqueKickL)*kfc + torqueBandR + torqueBandL + bw*w) / I;

end